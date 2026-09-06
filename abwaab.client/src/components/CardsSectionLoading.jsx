import { Skeleton } from "@mui/material";

export const CardsSectionLoading = () => {
  return (
    <div className="max-w-7xl text-start w-full my-10">
      <div className="flex justify-between items-center">
        <div className="">
          <Skeleton
            className="inline-block my-1"
            variant="rounded"
            width={"180px"}
            height={"40px"}
          />
          <Skeleton
            className="inline-block my-1"
            variant="rounded"
            width={"250px"}
            height={"40px"}
          />
        </div>
        <Skeleton
          className="inline-block my-1"
          variant="rounded"
          width={"100px"}
          height={"40px"}
        />
      </div>
      <div className="flex justify-between items-center gap-6">
        <Skeleton
          className="inline-block my-1"
          variant="rounded"
          width={"100%"}
          height={"350px"}
        />
        <Skeleton
          className="inline-block my-1"
          variant="rounded"
          width={"100%"}
          height={"350px"}
        />
        <Skeleton
          className="inline-block my-1"
          variant="rounded"
          width={"100%"}
          height={"350px"}
        />
      </div>
    </div>
  );
};
