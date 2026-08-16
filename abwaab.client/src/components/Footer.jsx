import React from "react";
import { Link } from "react-router";

const Footer = () => {
  const date = new Date();
  return (
    <div className="p-5 md:px-16 bg-navy-600 flex items-center text-white justify-between">
      <div className="text-sm">
        <ul className="list-none flex items-center gap-8">
          <li>
            <Link to="/contact-us" replace>
              تواصل معنا
            </Link>
          </li>
          <li>
            <Link to="/terms-and-conditions" replace>
              الشروط والأحكام
            </Link>
          </li>
          <li>
            <Link to="/privacy-policy" replace>
              سياسة الخصوصية
            </Link>
          </li>
        </ul>
      </div>
      <div className="">
        <p className="">&copy; {date.getFullYear()} أبواب</p>
      </div>
    </div>
  );
};

export default Footer;
