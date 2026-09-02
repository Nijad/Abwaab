export const PROPERTY_DETAILS = {
  propertyType: "",
  propertyFinishing: "",
  propertyMediaList: [],
  propertyAttributesList: [],
  viewsNumber: 0,
  propertyId: "",
  title: null,
  description: "",
  address: " ",
  areaInSquareMeter: null,
  price: null,
  latitude: null,
  longitude: null,
  isStar: false,
  propertyState: "",
};
export const ORIENTATIONS = [
  {
    attributeId: "b6c2c609-8240-4a8e-a57a-01b2e2c22f89",
    attributeName: "شرقي",
  },
  {
    attributeId: "28e3819e-cfd1-404b-9fed-09264b61a9ed",
    attributeName: "جنوبي",
  },
  {
    attributeId: "4d3568ee-436b-4a53-9601-261312622055",
    attributeName: "شمالي",
  },
  {
    attributeId: "539c754b-dae7-47fc-aa13-f48817ac3475",
    attributeName: "غربي",
  },
];
export const PROPERTY_AVAILABLE_DATETIME = [
  {
    dayNumber: -1,
    dayName: "",
    dayDate: "",
    dayTimes: [
      {
        startTime: "",
        endTime: "",
      },
    ],
  },
];
export const BOOK_APPOINTMENT = {
  propertyId: "",
  appointmentDate: "",
  endTime: "",
};

export const PROPERTY_UPDATE_DATA = {
  propertyId: "",
  title: "",
  description: "",
  address: "",
  areaInSquareMeter: 0,
  price: 0,
  latitude: 0,
  longitude: 0,
  isStar: false,
  propertyState: "",
  propertyTypeId: "",
  propertyFinishingId: "",
  timeSlots: [
    {
      timeSlotId: "",
      dayNumber: -1,
      dayName: "",
      startTime: "",
      endTime: "",
      notes: "",
    },
  ],
  propertyAttributesList: [
    {
      attributeId: "",
      attributeName: "",
      value: "",
      dataTypeDescription: "",
      propertyAttributeId: "",
      dataTypeId: "",
    },
  ],
  propertyMediaList: [
    {
      mediaId: "",
      filePath: "",
      mediaTypeName: "",
      isCover: true,
      mediaTypeId: "",
    },
  ],
};

export const PROPERTY_GET_DATA = {
  remainingStarsAllowed: 0,
  remainingImagesAllowed: 0,
  remainingVideosAllowed: 0,
  propertyTypeId: "",
  propertyFinishingId: "",
  timeSlots: [
    {
      timeSlotId: "",
      dayNumber: -1,
      dayName: "",
      startTime: "",
      endTime: "",
      notes: "",
    },
  ],
  propertyAttributesList: [
    {
      propertyAttributeId: "",
      dataTypeId: "",
      attributeId: "",
      attributeName: "",
      value: "",
      dataTypeDescription: "",
    },
  ],
  // propertyMediaList: [
  //   {
  //     mediaTypeId: "",
  //     mediaId: "",
  //     filePath: "",
  //     mediaTypeName: "",
  //     isCover: false,
  //   },
  // ],
  propertyId: "",
  title: "",
  description: "",
  address: "",
  areaInSquareMeter: null,
  price: 0,
  latitude: null,
  longitude: null,
  isStar: false,
  propertyState: "",
};

export const PROPERTY_GET_LISTS = {
  propertyTypesList: [
    {
      typeId: "",
      typeName: "",
    },
  ],
  propertyFinishingsList: [
    {
      finishingId: "",
      finishingName: "",
    },
  ],
  weekDaysList: [
    {
      dayIndex: -1,
      dayName: "",
    },
  ],
  attributes: [
    {
      attributeId: "",
      attributeName: "",
      dataTypeId: "",
      datayTypeDescription: "",
      possibleValues: [],
    },
  ],
  mediaTypes: [
    {
      mediaTypeId: "",
      mediaTypeName: "",
      isCover: false,
    },
  ],
};
export const PROPERTY_MEDIA = {
  mediaTypeId: "",
  mediaId: "",
  filePath: "",
  mediaTypeName: "",
  isCover: true,
};
