using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Queries.CartExistsById;
using Teashop.Backend.Application.Order.Repositories;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Application.Order.Commands.PlaceOrder
{
    public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
    {
        private readonly string[] _paymentCardNumberPatterns =
        {
            @"^(?:4[0-9]{12}(?:[0-9]{3})?)$",   // Visa
            @"^(?:5[1-5][0-9]{14})$",           // MasterCard
            @"^(?:3[47][0-9]{13})$"             // American Express
        };

        private readonly IMediator _mediator;
        private readonly ICountryRepository _countryRepository;
        private readonly IShippingMethodRepository _shippingMethodRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public PlaceOrderCommandValidator(
            IMediator mediator,
            ICountryRepository countryRepository,
            IShippingMethodRepository shippingMethodRepository,
            IPaymentMethodRepository paymentMethodRepository)
        {
            _mediator = mediator;
            _countryRepository = countryRepository;
            _shippingMethodRepository = shippingMethodRepository;
            _paymentMethodRepository = paymentMethodRepository;

            SetupRules();
        }

        private void SetupRules()
        {
            RuleFor(c => c.ContactInfo)
                .NotEmpty().WithMessage("Contact info is required.");
            When(c => c.ContactInfo != null, SetupContactInfoRules);

            RuleFor(c => c.ShippingAddress)
                .NotEmpty().WithMessage("Shipping address is required.");
            When(c => c.ShippingAddress != null, SetupShippingAddressRules);

            RuleFor(c => c.BillingAddress)
                .NotEmpty().WithMessage("Billing address is required.");
            When(c => c.ShippingAddress != null, SetupBillingAddressRules);

            RuleFor(c => c.ChosenShippingMethodName)
                .NotEmpty().WithMessage("Chosen shipping method name is required.")
                .MustAsync(BeNameOfExistingShippingMethod).WithMessage("Shipping method with given name does not exist.");

            RuleFor(c => c.ChosenPaymentMethodName)
                .NotEmpty().WithMessage("Chosen payment method name is required.")
                .MustAsync(BeNameOfExistingPaymentMethod).WithMessage("Payment method with given name does not exist.");
            When(c => c.ChosenPaymentMethodName == "card", SetupPaymentCardRules);

            RuleFor(c => c.CartId)
                .NotEmpty().WithMessage("Cart id is required.")
                .MustAsync(BeIdOfExistingCart).WithMessage("Cart with given id does not exist.");
        }

        private void SetupContactInfoRules()
        {
            RuleFor(c => c.ContactInfo.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is incorrect.");
        }

        private void SetupShippingAddressRules()
        {
            RuleFor(c => c.ShippingAddress).SetValidator(GetAddressValidator());
        }

        private void SetupBillingAddressRules()
        {
            RuleFor(c => c.BillingAddress).SetValidator(GetAddressValidator());
        }

        private void SetupPaymentCardRules()
        {
            RuleFor(c => c.PaymentCard)
                .NotEmpty().WithMessage("Payment card is required when chosen payment method is card.");

            RuleFor(c => c.PaymentCard.Number)
                .NotEmpty().WithMessage("Card number is required.")
                .Must(BeValidPaymentCardNumber).WithMessage("Card number is incorrect.");

            RuleFor(c => c.PaymentCard.Name)
                .NotEmpty().WithMessage("Card number is required.")
                .MinimumLength(2).WithMessage("Name on card must be at least 2 characters long.")
                .MaximumLength(26).WithMessage("Name on card must not less than 26 characters long.");

            RuleFor(c => c.PaymentCard.ExpirationDate)
                .NotEmpty().WithMessage("Card expiration date is required.")
                .Matches(@"^(0[1-9]|1[0-2])\/([0-9]{2})$").WithMessage("Card expiration date is incorrect.");

            RuleFor(c => c.PaymentCard.SecurityCode)
                .NotEmpty().WithMessage("Card security code is required.")
                .Matches(@"^[0-9]{3,4}$").WithMessage("Card security code is incorrect.");
        }

        private bool BeValidPaymentCardNumber(string cardNumber)
        {
            return _paymentCardNumberPatterns
                .Any(pattern => Regex.IsMatch(cardNumber, pattern));
        }

        private async Task<bool> BeNameOfExistingShippingMethod(string name, CancellationToken cancellationToken)
        {
            return await _shippingMethodRepository.ExistsByName(name);
        }

        private async Task<bool> BeNameOfExistingPaymentMethod(string name, CancellationToken cancellationToken)
        {
            return await _paymentMethodRepository.ExistsByName(name);
        }

        private async Task<bool> BeIdOfExistingCart(Guid input, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new CartExistsByIdQuery() { CartId = input });
        }

        private AddressValidator GetAddressValidator()
        {
            return new AddressValidator(_countryRepository);
        }

        private class AddressValidator : AbstractValidator<Address>
        {
            private readonly string _internationalPhoneNumberPattern =
                @"^((\+\d{1,3}(-| )?\(?\d\)?(-| )?\d{1,5})|(\(?\d{2,6}\)?))(-| )?(\d{3,4})(-| )?(\d{4})(( x| ext)\d{1,5}){0,1}$";

            private readonly ICountryRepository _countryRepository;

            public AddressValidator(ICountryRepository countryRepository)
            {

                _countryRepository = countryRepository;
                SetupRules();
            }

            private void SetupRules()
            {
                RuleFor(a => a.FirstName)
                    .NotEmpty().WithMessage("First name is required.")
                    .MaximumLength(255).WithMessage("First name must be less than 255 characters long.");

                RuleFor(a => a.LastName)
                    .NotEmpty().WithMessage("Last name is required.")
                    .MaximumLength(255).WithMessage("Last name must be less than 255 characters long.");

                RuleFor(a => a.Company)
                    .MaximumLength(255).WithMessage("Company name must be less than 255 characters long.");

                RuleFor(a => a.AddressLine1)
                    .NotEmpty().WithMessage("Address first line is required.")
                    .MaximumLength(255).WithMessage("Address line must be less than 255 characters long.");

                RuleFor(a => a.AddressLine2)
                    .MaximumLength(255).WithMessage("Address line must be less than 255 characters long.");

                RuleFor(a => a.PostalCode)
                    .NotEmpty().WithMessage("Postal code is required.")
                    .MaximumLength(255).WithMessage("Postal code must be less than 10 characters long.");

                RuleFor(a => a.City)
                    .NotEmpty().WithMessage("City is required.")
                    .MaximumLength(255).WithMessage("City name must be less than 255 characters long.");

                RuleFor(a => a.CountryCode)
                    .NotEmpty().WithMessage("Country code is required.")
                    .MustAsync(BeCodeOfExistingCountry).WithMessage("Country with given code was not found.");

                RuleFor(a => a.Phone)
                    .NotEmpty().WithMessage("Phone is required.")
                    .Must(BeValidPhoneNumber).WithMessage("Phone number is incorrect.");
            }

            private async Task<bool> BeCodeOfExistingCountry(string countryCode, CancellationToken cancellationToken)
            {
                return await _countryRepository.ExistsByCode(countryCode);
            }

            private bool BeValidPhoneNumber(string phoneNumber)
            {
                return Regex.IsMatch(phoneNumber, _internationalPhoneNumberPattern);
            }
        }
    }
}
