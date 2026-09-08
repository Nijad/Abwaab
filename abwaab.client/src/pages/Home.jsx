import { useEffect, useRef, useState } from "react";
import { HeroSlider } from "../components/HeroSlider";
import { PropertyCard } from "../components/PropertyCard";
import HomePageSection from "../components/HomePageSection";
import { useSnackbar } from "notistack";
import { useNavigate } from "react-router";
import { visitorApi } from "../api/visitorApi";
import { CardsSectionLoading } from "../components/CardsSectionLoading";

const Home = () => {
  const [data, setData] = useState(null);
  // const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();
  const navigate = useNavigate();

  const fetchProperties = async () => {
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await visitorApi.homePageProperties(
        signalRef.current.signal
      );
      //   enqueueSnackbar(resp.data.message, { variant: "success" });
      setData(resp.data);
    } catch (err) {
      if (err.detail) enqueueSnackbar(err.detail, { variant: "error" });
      if (!err.detail) enqueueSnackbar(err, { variant: "error" });
      //list related error codes
      // enqueueSnackbar(err.detail, { variant: "error" });
      // if (err.errorCode === "VALIDATION_FAILED") {
      //   setErrors(err.errors);
      //   return;
      // } else if (err.errorCode === "") {
      //   enqueueSnackbar(err.response.data.message, { variant: "error" });
      // }
    } finally {
      setLoading(false);
    }
  };

  const handleShowAll = (type) => {
    navigate(`properties/category/${type}`);
  };

  useEffect(() => {
    setTimeout(() => {
      fetchProperties();
    }, 0);

    return () => {
      if (signalRef.current) {
        signalRef.current.abort();
      }
    };
  }, []);

  return (
    <div className="w-full max-w-7xl mx-auto pt-5">
      <HeroSlider />
      {loading && (
        <div className="">
          <div className="">
            <CardsSectionLoading key={1} />
          </div>
          <div className="">
            <CardsSectionLoading key={2} />
          </div>
        </div>
      )}
      {!loading && (
        <HomePageSection
          key={"recent"}
          title="أضيفت حديثاً"
          description="استعرض العقارات المضافة حديثاً"
          type="recent-properties"
          showAllBtn={handleShowAll}
        >
          {data &&
            data.recentlyAddedList.map((item) => (
              <PropertyCard
                key={item.propertyId}
                area={item.area}
                currency="دولار امريكي"
                image={`${import.meta.env.VITE_API_BASE_URL}${item.coverImage}`}
                location={item.address}
                price={item.price}
                statusTag={item.propertyFinishing}
                title={item.title}
                typeTag={item.propertyType}
                onClick={() => navigate(`properties/${item.propertyId}`)}
              />
            ))}
        </HomePageSection>
      )}
      {!loading && (
        <HomePageSection
          key={"special"}
          title="العقارات المميزة"
          description="استعرض العقارات التي تم الترويج لها"
          type="promoted-properties"
          showAllBtn={handleShowAll}
        >
          {data &&
            data.premiumPropertiesList.map((item) => (
              <PropertyCard
                key={item.propertyId}
                area={item.area}
                currency="دولار امريكي"
                image={`${import.meta.env.VITE_API_BASE_URL}${item.coverImage}`}
                location={item.address}
                price={item.price}
                statusTag={item.propertyFinishing}
                title={item.title}
                typeTag={item.propertyType}
                onClick={() => navigate(`properties/${item.propertyId}`)}
              />
            ))}
        </HomePageSection>
      )}
      {!loading && (
        <HomePageSection
          key={"mostviews"}
          title="الأكثر  مشاهدةً"
          description="ألق نظرة على اكثر العقارات مشاهدة من الزوار"
          type="most-viewed"
          showAllBtn={handleShowAll}
        >
          {data &&
            data.mostViewedList.map((item) => (
              <PropertyCard
                key={item.propertyId}
                area={item.area}
                currency="دولار امريكي"
                image={`${import.meta.env.VITE_API_BASE_URL}${item.coverImage}`}
                location={item.address}
                price={item.price}
                statusTag={item.propertyFinishing}
                title={item.title}
                typeTag={item.propertyType}
                onClick={() => navigate(`properties/${item.propertyId}`)}
              />
            ))}
        </HomePageSection>
      )}
    </div>
  );
};

export default Home;
