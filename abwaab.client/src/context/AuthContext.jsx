// src/context/AuthContext.jsx
import React, {
  createContext,
  useState,
  useEffect,
  useCallback,
  useRef,
} from "react";
import { parseJwt } from "../utils/helpers";
import { authApi } from "../api";

const AuthContext = createContext(null);

// مدة الجلسة لعدم النشاط: 30 دقيقة بالمللي ثانية
const INACTIVITY_LIMIT = 30 * 60 * 1000; // 30 mins

// دالة بسيطة لفك تشفير JWT Token بدون مكتبات خارجية

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(null);
  const [codeRemainingSeconds, setCodeRemainingTime] = useState(0);
  const [loading, setLoading] = useState(true);

  const timerRef = useRef(null);
  console.log(user);

  useEffect(() => {
    const initialize = async () => {
      const storedRefreshToken = sessionStorage.getItem("refreshToken");
      console.log(storedRefreshToken);
      if (!storedRefreshToken) {
        setLoading(false);
        return;
      }
      try {
        const response = await authApi.refreshToken(storedRefreshToken);
        console.log(response.data);

        setToken({
          token: response.data.accessToken,
          refreshToken: response.data.refreshToken,
        });
        if (response.data.refreshToken) {
          sessionStorage.setItem("refreshToken", response.data.refreshToken);
        }
      } catch (error) {
        console.log(error);
        sessionStorage.removeItem("refreshToken");
        setUser(null);
        setToken(null);
      } finally {
        setLoading(false);
      }
    };
    // debugger;
    initialize();
  }, []);

  // 1. تسجيل الخروج
  const logout = useCallback((reason = "manual") => {
    setToken(null);
    setUser(null);
    if (timerRef.current) clearTimeout(timerRef.current);

    if (reason === "inactivity") {
      alert("تم إنهاء الجلسة تلقائياً لعدم النشاط لمدة 30 دقيقة.");
    }
  }, []);

  // 2. إعادة ضبط مؤقت الخمول (Inactivity Timer)
  const resetInactivityTimer = useCallback(() => {
    if (timerRef.current) clearTimeout(timerRef.current);

    // إذا كان المستخدم مسجلاً دخوله بالفعل، نبدأ المؤقت
    if (token) {
      timerRef.current = setTimeout(() => {
        logout("inactivity");
      }, INACTIVITY_LIMIT);
    }
  }, [token, logout]);

  // 3. قراءة بيانات الـ Token عند بدء التشغيل أو تحديثه
  useEffect(() => {
    const to = setTimeout(() => {
      if (token) {
        const decoded = parseJwt(token.token);
        if (decoded) {
          console.log(decoded);
          setUser({
            name: decoded[
              "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
            ]
              .split("@")[0]
              .toUpperCase(),
            // الدعم للأدوار (مستخدم أو مدير النظام)
            isAdmin:
              decoded[
                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
              ] === "Admin"
                ? true
                : false,
            identifier: decoded.LoginIdentifier,
          });
        }
      } else {
        setUser(null);
      }
      setLoading(false);
    }, 0);
    return () => clearTimeout(to);
  }, [token]);

  // 4. الاستماع لحركة المستخدم لتتبع الخمول
  useEffect(() => {
    if (!token) return;

    const events = ["mousemove", "keydown", "click", "scroll", "touchstart"];

    // تشغيل المؤقت أول مرة
    resetInactivityTimer();

    const handleUserActivity = () => {
      resetInactivityTimer();
    };

    // ربط الأحداث بالمتصفح
    events.forEach((event) =>
      window.addEventListener(event, handleUserActivity)
    );

    return () => {
      // تنظيف الأحداث عند الخروج
      events.forEach((event) =>
        window.removeEventListener(event, handleUserActivity)
      );
      if (timerRef.current) clearTimeout(timerRef.current);
    };
  }, [token, resetInactivityTimer]);

  // 5. دالة تسجيل الدخول
  const login = (info) => {
    console.log("work2");
    sessionStorage.setItem("refreshToken", info.refreshToken);
    setToken({ token: info.accessToken, refreshToken: info.refreshToken });
    // setUser({ name: info.userName, isAdmin: info.isAdmin });
  };

  const setRemainingSeconds = (date) => {
    setCodeRemainingTime(date);
  };
  // دالة للتحقق من الصلاحيات والـ Role المسموح
  // const isAdmin = (allowedRoles = []) => {
  //   if (!user) return false;
  //   return allowedRoles.includes(user.role);
  // };

  const setIdentifier = (identifier) => {
    setUser({ ...user, identifier });
  };
  return (
    <AuthContext.Provider
      value={{
        user,
        token,
        isAuthenticated: !!user,
        isAdmin: user?.isAdmin,
        login,
        logout,
        codeRemainingSeconds,
        setRemainingSeconds,
        loading,
        setLoading,
        setIdentifier,
      }}
    >
      {!loading ? children : <div>Loading Session{">>>>>"}</div>}
    </AuthContext.Provider>
  );
};
export default AuthContext;
