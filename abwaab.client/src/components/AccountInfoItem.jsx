import React from "react";

const AccountInfoItem = ({ title, info, action, verification }) => {
  return (
    <div className="w-full border-b border-neutral-300 py-4 flex items-center last:border-b-0 justify-between">
      {/* title and info */}
      <div className={`flex-1`}>
        <h4 className="font-semibold text-sm text-black">{title}</h4>
        <p
          className="text-base text-neutral-700 text-right"
          style={{ direction: "initial" }}
        >
          {info}
        </p>
      </div>
      {/* case if only action provided */}
      {action && !verification && <div className="w-[32%]">{action}</div>}
      {/* case if action and verification provided */}
      {action && verification && (
        <div className="flex w-[32%]">
          <div className="">{action}</div>
          <div className="">{verification}</div>
        </div>
      )}
    </div>
  );
};

export default AccountInfoItem;
