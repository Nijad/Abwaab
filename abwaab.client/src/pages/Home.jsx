import React from "react";
import { HeroSlider } from "../components/HeroSlider";
import { PropertyCard } from "../components/PropertyCard";

const Home = () => {
  return (
    <div>
      <HeroSlider />
      <div className="flex gap-7 justify-center m-5">
        <PropertyCard />
        <PropertyCard />
        <PropertyCard />
      </div>
    </div>
  );
};

export default Home;
