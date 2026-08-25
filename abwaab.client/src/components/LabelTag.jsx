const LabelTag = ({ classes, label }) => {
  return <p className={`p-1 w-fit rounded-2xl my-2 ${classes}`}>{label}</p>;
};

export default LabelTag;
