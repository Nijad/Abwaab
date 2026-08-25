using Abwaab.Application.Common.Enums;
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
                new MediaType { Id = new Guid(), Name = MediaTypesEnum.Video.ToString() },
                new MediaType { Id = new Guid(), Name = MediaTypesEnum.Image.ToString() }
            };
        }

        public static List<ServiceType> LoadServiceTypes()
        {
            return new List<ServiceType>
            {
                new ServiceType { Id = new Guid(), ServiceName = ServiceTypesEnum.Plan_Subscription.ToString().Replace("_", " ") },
                new ServiceType { Id = new Guid(), ServiceName = ServiceTypesEnum.Advertisment.ToString() }
            };
        }

        public static List<PaymentState> LoadPaymentStates()
        {
            return new List<PaymentState>
            {
                new PaymentState { Id = new Guid(), StateName = PaymentStatesEnum.Pending.ToString() },
                new PaymentState { Id = new Guid(), StateName = PaymentStatesEnum.Completed.ToString() },
                new PaymentState { Id = new Guid(), StateName = PaymentStatesEnum.Failed.ToString() },
                new PaymentState { Id = new Guid(), StateName = PaymentStatesEnum.Refunded.ToString() },
                new PaymentState { Id = new Guid(), StateName = PaymentStatesEnum.Expired.ToString() },
                new PaymentState { Id = new Guid(), StateName = PaymentStatesEnum.Cancelled.ToString() }
            };
        }

        public static List<AppointmentState> LoadAppointmentStates()
        {
            return new List<AppointmentState>
            {
                new AppointmentState { Id = new Guid(), StateName = AppointmentStatesEnum.Pending.ToString() },
                new AppointmentState { Id = new Guid(), StateName = AppointmentStatesEnum.Refused.ToString() },
                new AppointmentState { Id = new Guid(), StateName = AppointmentStatesEnum.Accepted.ToString() },
                new AppointmentState { Id = new Guid(), StateName = AppointmentStatesEnum.Canceled.ToString() },
                new AppointmentState { Id = new Guid(), StateName = AppointmentStatesEnum.Unfinished.ToString() },
                new AppointmentState { Id = new Guid(), StateName = AppointmentStatesEnum.Completed.ToString() }
            };
        }

        public static List<NotificationState> LoadNotificationStates()
        {
            return new List<NotificationState>
            {
                new NotificationState { Id = new Guid(), StateName = NotificationStatesEnum.Pending.ToString() },
                new NotificationState { Id = new Guid(), StateName = NotificationStatesEnum.Sent.ToString() },
                new NotificationState { Id = new Guid(), StateName = NotificationStatesEnum.Failed.ToString() },
                new NotificationState { Id = new Guid(), StateName = NotificationStatesEnum.Unread.ToString() },
                new NotificationState { Id = new Guid(), StateName = NotificationStatesEnum.Read.ToString() },
            };
        }

        public static List<NotificationWay> LoadNotificationWays()
        {
            return new List<NotificationWay>
            {
                new NotificationWay
                {
                    Id = new Guid(),
                    WayName = NotificationWaysEnum.Email.ToString(),
                    CanDisable = true,
                },
                new NotificationWay
                {
                    Id = new Guid(),
                    WayName = NotificationWaysEnum.SMS.ToString(),
                    CanDisable = true
                },
                new NotificationWay
                {
                    Id = new Guid(),
                    WayName = NotificationWaysEnum.Web_Application.ToString(),
                    CanDisable = false
                }
            };
        }

        public static List<AppointmentAction> LoadAppointmentActions()
        {
            return new List<AppointmentAction>
            {
                new AppointmentAction { Id = new Guid(), ActionName = AppointmentActionsEnum.Request.ToString() },
                new AppointmentAction { Id = new Guid(), ActionName = AppointmentActionsEnum.Accept.ToString() },
                new AppointmentAction { Id = new Guid(), ActionName = AppointmentActionsEnum.Refuse.ToString() },
                new AppointmentAction { Id = new Guid(), ActionName = AppointmentActionsEnum.Cancel.ToString() },
                new AppointmentAction { Id = new Guid(), ActionName = AppointmentActionsEnum.Visit.ToString() },
                new AppointmentAction { Id = new Guid(), ActionName = AppointmentActionsEnum.Report.ToString() },
            };
        }

        public static List<PropertyState> LoadPropertyStates()
        {
            return new List<PropertyState>
            {
                new PropertyState { Id = new Guid(), StateName = PropertyStatesEnum.Preparing.ToString() },
                new PropertyState { Id = new Guid(), StateName = PropertyStatesEnum.Pending.ToString() },
                new PropertyState { Id = new Guid(), StateName = PropertyStatesEnum.Published.ToString() },
                new PropertyState { Id = new Guid(), StateName = PropertyStatesEnum.Rejected.ToString() },
                new PropertyState { Id = new Guid(), StateName = PropertyStatesEnum.Sold.ToString() },
                new PropertyState { Id = new Guid(), StateName = PropertyStatesEnum.Deleted.ToString() },
                new PropertyState { Id = new Guid(), StateName = PropertyStatesEnum.Disabled.ToString() }
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
                new Finishing { Id = new Guid(), FinishingName = "جديد" },
                new Finishing { Id = new Guid(), FinishingName = "جيد" },
                new Finishing { Id = new Guid(), FinishingName = "قديم" },
                new Finishing { Id = new Guid(), FinishingName = "ممتاز" }
            };
        }

        public static List<PropertyType> LoadPropertyTypes()
        {
            return new List<PropertyType>
            {
                new PropertyType { Id = new Guid(), TypeName = "شقة" },
                new PropertyType { Id = new Guid(), TypeName = "منزل" },
                new PropertyType { Id = new Guid(), TypeName = "فيلا" },
                new PropertyType { Id = new Guid(), TypeName = "أرض" }
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
                    AttributeDataTypeId = new Guid("937c0e65-b4ec-47f2-8aa9-d758603e20a5")
                },

                new Attribute
                {
                    Id = new Guid("6c0851d8-fc13-465c-94ab-373c25e9228b"),
                    AttributeName = "Number of Bathrooms",
                    AttributeDataTypeId = new Guid("937c0e65-b4ec-47f2-8aa9-d758603e20a5")
                },

                new Attribute
                {
                    Id = new Guid("2988ec33-46ff-4301-9f67-4e69fc700875"),
                    AttributeName = "Floor Number",
                    AttributeDataTypeId = new Guid("937c0e65-b4ec-47f2-8aa9-d758603e20a5")
                },

                new Attribute {
                    Id = new Guid("2b0ffaba-bd0d-4a3b-a263-d0b61a9cb2f4"),
                    AttributeName = "Has Garage",
                    AttributeDataTypeId = new Guid("883a9f1e-e227-4079-a929-f0aa0ff40990")
                },

                new Attribute {
                    Id = new Guid("75d70597-6f68-46a8-9056-39622952fd10"),
                    AttributeName = "Has Garden",
                    AttributeDataTypeId = new Guid("883a9f1e-e227-4079-a929-f0aa0ff40990")
                },

                new Attribute {
                    Id = new Guid("158bfbe6-46c7-4f0c-adc0-f2059b0c64b1"),
                    AttributeName = "Has Swimming Pool",
                    AttributeDataTypeId = new Guid("883a9f1e-e227-4079-a929-f0aa0ff40990")
                },

                new Attribute {
                    Id = new Guid("b6c2c609-8240-4a8e-a57a-01b2e2c22f89"),
                    AttributeName = "شرقي",
                    AttributeDataTypeId = new Guid("883a9f1e-e227-4079-a929-f0aa0ff40990")
                },

                new Attribute {
                    Id = new Guid("28e3819e-cfd1-404b-9fed-09264b61a9ed"),
                    AttributeName = "جنوبي",
                    AttributeDataTypeId = new Guid("883a9f1e-e227-4079-a929-f0aa0ff40990")
                },

                new Attribute {
                    Id = new Guid("4d3568ee-436b-4a53-9601-261312622055"),
                    AttributeName = "شمالي",
                    AttributeDataTypeId = new Guid("883a9f1e-e227-4079-a929-f0aa0ff40990")
                },

                new Attribute {
                    Id = new Guid("539c754b-dae7-47fc-aa13-f48817ac3475"),
                    AttributeName = "غربي",
                    AttributeDataTypeId = new Guid("883a9f1e-e227-4079-a929-f0aa0ff40990")
                }
            };
        }

        public static List<AttributeDataType> LoadAttributeDataTypeValues()
        {
            return new List<AttributeDataType>
            {
                new AttributeDataType { Id = new Guid("85ecffc0-07cf-4bc8-aed7-423bdb8738ee"), Name = AttributeDataTypeEnum.text.ToString() },
                new AttributeDataType { Id = new Guid("937c0e65-b4ec-47f2-8aa9-d758603e20a5"), Name = AttributeDataTypeEnum.number.ToString() },
                new AttributeDataType { Id = new Guid("883a9f1e-e227-4079-a929-f0aa0ff40990"), Name = AttributeDataTypeEnum.boolean.ToString() },
                new AttributeDataType { Id = new Guid("f374db99-2e52-4152-958b-fb1f3e24eaf5"), Name = AttributeDataTypeEnum.list.ToString() }
            };
        }
/*
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
*/
        public static List<UserPlanStatus> LoadUserPlanStates()
        {
            return new List<UserPlanStatus>()
            {
                new UserPlanStatus() { Id = new Guid(), StateName = UserPlanStatesEnum.Pending.ToString() },
                new UserPlanStatus() { Id = new Guid(), StateName = UserPlanStatesEnum.Active.ToString() },
                new UserPlanStatus() { Id = new Guid(), StateName = UserPlanStatesEnum.Expiered.ToString() },
                new UserPlanStatus() { Id = new Guid(), StateName = UserPlanStatesEnum.Working.ToString() },
                new UserPlanStatus() { Id = new Guid(), StateName = UserPlanStatesEnum.Canceled.ToString() },
            };
        }
    }
}