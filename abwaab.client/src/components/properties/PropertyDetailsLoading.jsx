import { Skeleton } from "@mui/material";

const PropertyDetailsLoading = () => {
  return (
    <div className="bg-neutral-50 flex flex-col max-w-7x px-28 mx-auto mt-5 pb-24">
      <div className="flex items-center content-between w-full my-4 gap-3">
        <div className="w-2/3 flex-1">
          <Skeleton variant="rounded" width={410} height={50} />
        </div>
        <div className="w-1/3 text-end">
          <Skeleton variant="rounded" width={210} height={50} />
        </div>
      </div>
      <div className="flex content-between gap-5">
        <div className="w-2/3">
          <Skeleton
            className="my-4"
            variant="rounded"
            width={"100%"}
            height={400}
          />
          <Skeleton
            className="my-4"
            variant="rounded"
            width={"100%"}
            height={150}
          />
          <Skeleton
            className="my-4"
            variant="rounded"
            width={"100%"}
            height={150}
          />
        </div>
        <div className="w-1/3">
          <Skeleton
            className="my-4"
            variant="rounded"
            width={"100%"}
            height={550}
          />
        </div>
      </div>
    </div>
  );
};

export default PropertyDetailsLoading;
