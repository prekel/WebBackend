import { Record } from "../.fable/fable-library.3.1.11/Types.js";
import { array_type, bool_type, option_type, record_type, class_type, string_type, int32_type } from "../.fable/fable-library.3.1.11/Reflection.js";

export class ProductDto extends Record {
    constructor(productId, name, description, price) {
        super();
        this.productId = (productId | 0);
        this.name = name;
        this.description = description;
        this.price = price;
    }
}

export function ProductDto$reflection() {
    return record_type("MyStore.Dto.Shop.ProductDto", [], ProductDto, () => [["productId", int32_type], ["name", string_type], ["description", string_type], ["price", class_type("System.Decimal")]]);
}

export class CustomerDto extends Record {
    constructor(customerId, firstName, lastName, honorific, email, userId, currentCartId) {
        super();
        this.customerId = (customerId | 0);
        this.firstName = firstName;
        this.lastName = lastName;
        this.honorific = honorific;
        this.email = email;
        this.userId = userId;
        this.currentCartId = currentCartId;
    }
}

export function CustomerDto$reflection() {
    return record_type("MyStore.Dto.Shop.CustomerDto", [], CustomerDto, () => [["customerId", int32_type], ["firstName", string_type], ["lastName", option_type(string_type)], ["honorific", option_type(string_type)], ["email", string_type], ["userId", option_type(string_type)], ["currentCartId", option_type(int32_type)]]);
}

export class CartDto extends Record {
    constructor(cartId, isPublic, ownerCustomerId) {
        super();
        this.cartId = (cartId | 0);
        this.isPublic = isPublic;
        this.ownerCustomerId = ownerCustomerId;
    }
}

export function CartDto$reflection() {
    return record_type("MyStore.Dto.Shop.CartDto", [], CartDto, () => [["cartId", int32_type], ["isPublic", bool_type], ["ownerCustomerId", option_type(int32_type)]]);
}

export class OrderDto extends Record {
    constructor(orderId, customerId, createTimeOffset) {
        super();
        this.orderId = (orderId | 0);
        this.customerId = (customerId | 0);
        this.createTimeOffset = createTimeOffset;
    }
}

export function OrderDto$reflection() {
    return record_type("MyStore.Dto.Shop.OrderDto", [], OrderDto, () => [["orderId", int32_type], ["customerId", int32_type], ["createTimeOffset", class_type("System.DateTimeOffset")]]);
}

export class OrderedProductDto extends Record {
    constructor(productId, orderId, orderedPrice) {
        super();
        this.productId = (productId | 0);
        this.orderId = (orderId | 0);
        this.orderedPrice = orderedPrice;
    }
}

export function OrderedProductDto$reflection() {
    return record_type("MyStore.Dto.Shop.OrderedProductDto", [], OrderedProductDto, () => [["productId", int32_type], ["orderId", int32_type], ["orderedPrice", class_type("System.Decimal")]]);
}

export class CartModel extends Record {
    constructor(cart, products) {
        super();
        this.cart = cart;
        this.products = products;
    }
}

export function CartModel$reflection() {
    return record_type("MyStore.Dto.Shop.CartModel", [], CartModel, () => [["cart", CartDto$reflection()], ["products", array_type(ProductDto$reflection())]]);
}

export class OrderModel extends Record {
    constructor(order, orderedProducts) {
        super();
        this.order = order;
        this.orderedProducts = orderedProducts;
    }
}

export function OrderModel$reflection() {
    return record_type("MyStore.Dto.Shop.OrderModel", [], OrderModel, () => [["order", OrderDto$reflection()], ["orderedProducts", array_type(OrderedProductDto$reflection())]]);
}

export class ProductsModel extends Record {
    constructor(products) {
        super();
        this.products = products;
    }
}

export function ProductsModel$reflection() {
    return record_type("MyStore.Dto.Shop.ProductsModel", [], ProductsModel, () => [["products", array_type(ProductDto$reflection())]]);
}

