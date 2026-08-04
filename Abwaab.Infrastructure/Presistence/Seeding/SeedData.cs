using Abwaab.Domain.Entities.AppointmentEntities;
using Abwaab.Domain.Entities.MediaEntities;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.PaymentEntities;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Infrastructure.Presistence.Seeding
{
    public static class SeedData
    {
        public static List<MediaType> LoadMediaTypes()
        {
            return new List<MediaType>
            {
                new MediaType { Id = new Guid(), Name = "Video" },
                new MediaType { Id = new Guid(), Name = "Image" }
            };
        }

        public static List<ServiceType> LoadServiceTypes()
        {
            return new List<ServiceType>
            {
                new ServiceType { Id = new Guid(), ServiceName = "Plan Subscription" },
                new ServiceType { Id = new Guid(), ServiceName = "Advertisment" }
            };
        }

        public static List<PaymentState> LoadPaymentStates()
        {
            return new List<PaymentState>
            {
                new PaymentState { Id = new Guid(), StateName = "Pending" },
                new PaymentState { Id = new Guid(), StateName = "Completed" },
                new PaymentState { Id = new Guid(), StateName = "Failed" }
            };
        }

        public static List<AppointmentState> LoadAppointmentStates()
        {
            return new List<AppointmentState>
            {
                new AppointmentState { Id = new Guid(), StateName = "Pending" },
                new AppointmentState { Id = new Guid(), StateName = "Refused" },
                new AppointmentState { Id = new Guid(), StateName = "Accepted" },
                new AppointmentState { Id = new Guid(), StateName = "Canceled" },
                new AppointmentState { Id = new Guid(), StateName = "Unfinished" },
                new AppointmentState { Id = new Guid(), StateName = "Completed" }
            };
        }

        public static List<NotificationState> LoadNotificationStates()
        {
            return new List<NotificationState>
            {
                new NotificationState { Id = new Guid(), StateName = "Pending" },
                new NotificationState { Id = new Guid(), StateName = "Sent" },
                new NotificationState { Id = new Guid(), StateName = "Failed" },
                new NotificationState { Id = new Guid(), StateName = "Unread" },
                new NotificationState { Id = new Guid(), StateName = "Read" },
            };
        }

        public static List<NotificationWay> LoadNotificationWays()
        {
            return new List<NotificationWay>
            {
                new NotificationWay { Id = new Guid(), WayName = "Email" },
                new NotificationWay { Id = new Guid(), WayName = "SMS" },
                new NotificationWay { Id = new Guid(), WayName = "Push Notification" }
            };
        }

        public static List<AppointmentAction> LoadAppointmentActions()
        {
            return new List<AppointmentAction>
            {
                new AppointmentAction { Id = new Guid(), ActionName = "Request" },
                new AppointmentAction { Id = new Guid(), ActionName = "Accept" },
                new AppointmentAction { Id = new Guid(), ActionName = "Refuse" },
                new AppointmentAction { Id = new Guid(), ActionName = "Cancel" },
                new AppointmentAction { Id = new Guid(), ActionName = "Visit" },
                new AppointmentAction { Id = new Guid(), ActionName = "Report" },
            };
        }

        public static List<PropertyState> LoadPropertyStates()
        {
            return new List<PropertyState>
            {
                new PropertyState { Id = new Guid(), StateName = "Pending" },
                new PropertyState { Id = new Guid(), StateName = "Published" },
                new PropertyState { Id = new Guid(), StateName = "Rejected" },
                new PropertyState { Id = new Guid(), StateName = "Sold" },
                new PropertyState { Id = new Guid(), StateName = "Deleted" },
                new PropertyState { Id = new Guid(), StateName = "Disabled" }
            };
        }

        public static List<Plan> LoadPlans()
        {
            return new List<Plan>
            {
                new Plan {
                    Id = new Guid(),
                    Name = "Basic Plan",
                    Price = 0m,
                    DurationInDays = 3650,
                    StartDate = DateOnly.FromDateTime(DateTime.Now),
                    ExpieryDate = DateOnly.FromDateTime(DateTime.Now.AddYears(50)),
                    TempDurationInDays = 3650,
                    MaxPropertiesCountAtSameTime = 0,
                    MaxStardPropertiesCountAtSameTime = 0,
                    MaxImagesCount = 0,
                    MaxVideosCount = 0,
                    IsDisabled = false,
                    DefaultPlan = true
                }
            };
        }

        public static List<Finishing> LoadFinishings()
        {
            return new List<Finishing>
            {
                new Finishing { Id = new Guid(), FinishingName = "New" },
                new Finishing { Id = new Guid(), FinishingName = "Good" },
                new Finishing { Id = new Guid(), FinishingName = "Old" }
            };
        }

        public static List<PropertyType> LoadPropertyTypes()
        {
            return new List<PropertyType>
            {
                new PropertyType { Id = new Guid(), TypeName = "Apartment" },
                new PropertyType { Id = new Guid(), TypeName = "House" },
                new PropertyType { Id = new Guid(), TypeName = "Villa" },
                new PropertyType { Id = new Guid(), TypeName = "Land" }
            };
        }

        public static List<Attribute> LoadAttributes()
        {
            return new List<Attribute>
            {
                new Attribute 
                {
                    Id = new Guid("2f4afcee-5179-4a65-ab34-0a7641f0c5b1"),
                    AttributeName = "Number of Rooms",
                    DataType = AttributeDataType.number
                },

                new Attribute 
                { 
                    Id = new Guid("6c0851d8-fc13-465c-94ab-373c25e9228b"), 
                    AttributeName = "Number of Bathrooms",
                    DataType = AttributeDataType.number
                },

                new Attribute 
                { 
                    Id = new Guid("2988ec33-46ff-4301-9f67-4e69fc700875"), 
                    AttributeName = "Floor Number",
                    DataType = AttributeDataType.number
                },

                new Attribute { 
                    Id = new Guid("2b0ffaba-bd0d-4a3b-a263-d0b61a9cb2f4"), 
                    AttributeName = "Has Garage",
                    DataType = AttributeDataType.possibleValues
                },

                new Attribute { 
                    Id = new Guid("75d70597-6f68-46a8-9056-39622952fd10"), 
                    AttributeName = "Has Garden",
                    DataType = AttributeDataType.possibleValues
                },

                new Attribute { 
                    Id = new Guid("158bfbe6-46c7-4f0c-adc0-f2059b0c64b1"), 
                    AttributeName = "Has Swimming Pool",
                    DataType = AttributeDataType.possibleValues
                }
            };
        }

        public static List<AttributePossibleValue> LoadAttributePossibleValues()
        {
            return new List<AttributePossibleValue>
            {
                new AttributePossibleValue {
                    Id = new Guid(),
                    Value = "Yes",
                    AttributeId = new Guid("2b0ffaba-bd0d-4a3b-a263-d0b61a9cb2f4") // Has Garage
                },

                new AttributePossibleValue {
                    Id = new Guid(),
                    Value = "No",
                    AttributeId = new Guid("2b0ffaba-bd0d-4a3b-a263-d0b61a9cb2f4") // Has Garage
                },

                new AttributePossibleValue {
                    Id = new Guid(),
                    Value = "Yes",
                    AttributeId = new Guid("75d70597-6f68-46a8-9056-39622952fd10") // Has Garden
                },

                new AttributePossibleValue {
                    Id = new Guid(),
                    Value = "No",
                    AttributeId = new Guid("75d70597-6f68-46a8-9056-39622952fd10") // Has Garden
                },

                new AttributePossibleValue {
                    Id = new Guid(),
                    Value = "Yes",
                    AttributeId = new Guid("158bfbe6-46c7-4f0c-adc0-f2059b0c64b1") // Has Swimming Pool
                },

                new AttributePossibleValue {
                    Id = new Guid(),
                    Value = "No",
                    AttributeId = new Guid("158bfbe6-46c7-4f0c-adc0-f2059b0c64b1") // Has Swimming Pool
                }
            };
        }
    }
}