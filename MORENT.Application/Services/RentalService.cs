using MORENT.Application.Common;
using MORENT.Application.DTOs;
using MORENT.Application.Interfaces.Persistence;
using MORENT.Application.Interfaces.Services;
using MORENT.Domain.Entities.Dbo;
using MORENT.Domain.Enums;

namespace MORENT.Application.Services
{
    public class RentalService : IRentalService
    {
        private readonly IUnitOfWork _uow;

        public RentalService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<Guid>> CreateRentalAsync(CreateRentalRequestDto request, Guid userId)
        {
            // 1. Validate Dates
            if (request.DropOffDate <= request.PickUpDate)
            {
                return Result<Guid>.Failure("Drop-off date must be after pick-up date.");
            }

            // 2. Check Car Availability
            var isAvailable = await _uow.Cars.IsCarAvailableAsync(request.CarId, request.PickUpLocationId, request.PickUpDate, request.DropOffDate);
            if (!isAvailable)
            {
                return Result<Guid>.Failure("The selected car is not available at this location for the chosen dates.");
            }

            // 3. Fetch Car to calculate pricing
            var car = await _uow.Cars.GetByIdAsync(request.CarId);
            if (car == null) return Result<Guid>.Failure("Car not found.");

            // 4. Calculate Duration
            var rentalDays = Math.Ceiling((request.DropOffDate - request.PickUpDate).TotalDays);
            if (rentalDays < 1) rentalDays = 1;

            decimal subtotal = (decimal)rentalDays * car.PricePerDay;
            decimal discountAmount = 0;
            Guid? appliedPromoId = null;

            // 5. Promo Code Logic
            if (!string.IsNullOrWhiteSpace(request.PromoCode))
            {
                var promo = await _uow.PromoCodes.GetByCodeAsync(request.PromoCode);

                if (promo == null || !promo.IsActive || promo.ExpiresAt < DateTime.UtcNow)
                {
                    return Result<Guid>.Failure("The promo code is invalid or has expired.");
                }

                appliedPromoId = promo.Id;

                // Percentage or Amount
                if (promo.DiscountPercentage.HasValue)
                {
                    discountAmount = subtotal * (promo.DiscountPercentage.Value / 100m);
                }
                else if (promo.DiscountAmount.HasValue)
                {
                    discountAmount = promo.DiscountAmount.Value;
                }

                // Ensured that discount doesn't make subtotal negative
                if (discountAmount > subtotal) discountAmount = subtotal;
            }

            // 6. Tax Calculation
            decimal taxRate = 0.15m; // 15% Tax default
            decimal taxAmount = (subtotal - discountAmount) * taxRate;
            decimal totalAmount = (subtotal - discountAmount) + taxAmount;

            // 7. Create Rental Record
            var rental = new Rental
            {
                UserId = userId,
                CarId = request.CarId,
                PickUpLocationId = request.PickUpLocationId,
                DropOffLocationId = request.DropOffLocationId,
                PickUpDate = request.PickUpDate,
                DropOffDate = request.DropOffDate,
                PaymentMethodId = request.PaymentMethodId,
                RentalStatusId = (int)RentalStatusEnum.Confirmed,
                Subtotal = subtotal,
                Discount = discountAmount,
                Tax = taxAmount,
                TotalAmount = totalAmount,
                PromoCodeId = appliedPromoId
            };

            await _uow.Rentals.AddAsync(rental);
            await _uow.SaveChangesAsync();

            return Result<Guid>.Success(rental.Id);
        }

        public async Task<Result<RentalDto>> GetRentalDetailsAsync(Guid rentalId, Guid userId)
        {
            var rental = await _uow.Rentals.GetRentalWithDetailsAsync(rentalId);

            if (rental == null)
            {
                return Result<RentalDto>.Failure("Rental not found.");
            }

            // Ensure the rental belongs to the requesting user
            if (rental.UserId != userId) return Result<RentalDto>.Failure("Unauthorized.");

            return Result<RentalDto>.Success(rental);
        }
    }
}