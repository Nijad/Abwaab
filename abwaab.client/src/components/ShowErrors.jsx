import React from "react";

const ShowErrors = ({ object, key }) => {
  try {
    var values = object[key];
  } catch (error) {
    values = null;
  }
  return (
    <>
      {values &&
        values.map((e) => <span className="text-error-600 block">{e}</span>)}
    </>
  );
};

export default ShowErrors;
