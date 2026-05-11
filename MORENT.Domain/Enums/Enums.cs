namespace MORENT.Domain.Enums
{
    public enum SystemRoleEnum
    {
        Admin = 1,
        Customer = 2
    }

    public enum RentalStatusEnum
    {
        Confirmed = 1, // Upcoming and current active rentals
        Completed = 2,
        Cancelled = 3
    }

    public enum SteeringTypeEnum
    {
        Manual = 1,
        Automatic = 2
    }

    public enum CarTypeEnum
    {
        Sport = 1,
        SUV = 2,
        MPV = 3,
        Sedan = 4,
        Coupe = 5,
        Hatchback = 6
    }

    public enum FuelTypeEnum
    {
        Gasoline = 1,
        Electric = 2
    }

    public enum PaymentMethodEnum
    {
        CreditCard = 1,
        PayPal = 2,
        Bitcoin = 3
    }
}