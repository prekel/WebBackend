import { Record } from "../.fable/fable-library.3.1.11/Types.js";
import { bool_type, option_type, record_type, class_type, string_type, int32_type } from "../.fable/fable-library.3.1.11/Reflection.js";
import { toNumber } from "../.fable/fable-library.3.1.11/Decimal.js";
import Decimal from "../.fable/fable-library.3.1.11/Decimal.js";
import { OrderDto, CartDto, CustomerDto, ProductDto } from "./Dto/Shop.js";
import { toNullable, map, ofNullable } from "../.fable/fable-library.3.1.11/Option.js";

export class Product extends Record {
    constructor(ProductId, Name, Description, Price) {
        super();
        this.ProductId = (ProductId | 0);
        this.Name = Name;
        this.Description = Description;
        this.Price = Price;
    }
}

export function Product$reflection() {
    return record_type("MyStore.Domain.Shop.Product", [], Product, () => [["ProductId", int32_type], ["Name", string_type], ["Description", string_type], ["Price", class_type("System.Decimal")]]);
}

export function Product_ToDomain_Z6B097A11(dto) {
    return new Product(dto.productId, dto.name, dto.description, new Decimal(dto.price));
}

export function Product__FromDomain(this$) {
    return new ProductDto(this$.ProductId, this$.Name, this$.Description, toNumber(this$.Price));
}

export class Customer extends Record {
    constructor(CustomerId, FirstName, LastName, Honorific, Email, UserId, CurrentCartId) {
        super();
        this.CustomerId = (CustomerId | 0);
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Honorific = Honorific;
        this.Email = Email;
        this.UserId = UserId;
        this.CurrentCartId = CurrentCartId;
    }
}

export function Customer$reflection() {
    return record_type("MyStore.Domain.Shop.Customer", [], Customer, () => [["CustomerId", int32_type], ["FirstName", string_type], ["LastName", option_type(string_type)], ["Honorific", option_type(string_type)], ["Email", string_type], ["UserId", option_type(string_type)], ["CurrentCartId", option_type(int32_type)]]);
}

export function Customer_ToDomain_Z54B26620(dto) {
    return new Customer(dto.customerId, dto.firstName, ofNullable(dto.lastName), ofNullable(dto.honorific), dto.email, map((x_6) => x_6, ofNullable(dto.userId)), map((x_10) => x_10, ofNullable(dto.currentCartId)));
}

export function Customer__FromDomain(this$) {
    return new CustomerDto(this$.CustomerId, this$.FirstName, toNullable(this$.LastName), toNullable(this$.Honorific), this$.Email, toNullable(map((x_6) => x_6, this$.UserId)), toNullable(map((x_10) => x_10, this$.CurrentCartId)));
}

export class Cart extends Record {
    constructor(CartId, IsPublic, OwnerCustomerId) {
        super();
        this.CartId = (CartId | 0);
        this.IsPublic = IsPublic;
        this.OwnerCustomerId = OwnerCustomerId;
    }
}

export function Cart$reflection() {
    return record_type("MyStore.Domain.Shop.Cart", [], Cart, () => [["CartId", int32_type], ["IsPublic", bool_type], ["OwnerCustomerId", option_type(int32_type)]]);
}

export function Cart_ToDomain_268DEFC0(dto) {
    return new Cart(dto.cartId, dto.isPublic, map((x_3) => x_3, ofNullable(dto.ownerCustomerId)));
}

export function Cart__FromDomain(this$) {
    return new CartDto(this$.CartId, this$.IsPublic, toNullable(map((x_3) => x_3, this$.OwnerCustomerId)));
}

export class Order extends Record {
    constructor(OrderId, CustomerId, CreateTimeOffset) {
        super();
        this.OrderId = (OrderId | 0);
        this.CustomerId = (CustomerId | 0);
        this.CreateTimeOffset = CreateTimeOffset;
    }
}

export function Order$reflection() {
    return record_type("MyStore.Domain.Shop.Order", [], Order, () => [["OrderId", int32_type], ["CustomerId", int32_type], ["CreateTimeOffset", class_type("System.DateTimeOffset")]]);
}

export function Order_ToDomain_4E75080A(dto) {
    return new Order(dto.orderId, dto.customerId, dto.createTimeOffset);
}

export function Order__FromDomain(this$) {
    return new OrderDto(this$.OrderId, this$.CustomerId, this$.CreateTimeOffset);
}

