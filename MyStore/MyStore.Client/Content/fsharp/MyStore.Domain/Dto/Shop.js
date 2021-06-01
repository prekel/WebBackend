import { Record } from "../../.fable/fable-library.3.1.11/Types.js";
import { array_type, bool_type, class_type, record_type, float64_type, string_type, int32_type } from "../../.fable/fable-library.3.1.11/Reflection.js";

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
    return record_type("MyStore.Dto.Shop.ProductDto", [], ProductDto, () => [["productId", int32_type], ["name", string_type], ["description", string_type], ["price", float64_type]]);
}

export class CustomerDto extends Record {
    constructor(customerId, firstName, lastName, honorific, email, currentCartId) {
        super();
        this.customerId = (customerId | 0);
        this.firstName = firstName;
        this.lastName = lastName;
        this.honorific = honorific;
        this.email = email;
        this.currentCartId = currentCartId;
    }
}

export function CustomerDto$reflection() {
    return record_type("MyStore.Dto.Shop.CustomerDto", [], CustomerDto, () => [["customerId", int32_type], ["firstName", string_type], ["lastName", string_type], ["honorific", string_type], ["email", string_type], ["currentCartId", class_type("System.Nullable`1", [int32_type])]]);
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
    return record_type("MyStore.Dto.Shop.CartDto", [], CartDto, () => [["cartId", int32_type], ["isPublic", bool_type], ["ownerCustomerId", class_type("System.Nullable`1", [int32_type])]]);
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
    return record_type("MyStore.Dto.Shop.OrderedProductDto", [], OrderedProductDto, () => [["productId", int32_type], ["orderId", int32_type], ["orderedPrice", float64_type]]);
}

export class CartModel extends Record {
    constructor(cart, products, isCurrent) {
        super();
        this.cart = cart;
        this.products = products;
        this.isCurrent = isCurrent;
    }
}

export function CartModel$reflection() {
    return record_type("MyStore.Dto.Shop.CartModel", [], CartModel, () => [["cart", CartDto$reflection()], ["products", array_type(ProductDto$reflection())], ["isCurrent", bool_type]]);
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

export class CartsQuery extends Record {
    constructor(isPublic, count, offset) {
        super();
        this.isPublic = isPublic;
        this.count = (count | 0);
        this.offset = (offset | 0);
    }
}

export function CartsQuery$reflection() {
    return record_type("MyStore.Dto.Shop.CartsQuery", [], CartsQuery, () => [["isPublic", bool_type], ["count", int32_type], ["offset", int32_type]]);
}

export class CartsModel extends Record {
    constructor(carts, query) {
        super();
        this.carts = carts;
        this.query = query;
    }
}

export function CartsModel$reflection() {
    return record_type("MyStore.Dto.Shop.CartsModel", [], CartsModel, () => [["carts", array_type(CartDto$reflection())], ["query", CartsQuery$reflection()]]);
}

export class SetCurrentCartQuery extends Record {
    constructor(setCurrent) {
        super();
        this.setCurrent = setCurrent;
    }
}

export function SetCurrentCartQuery$reflection() {
    return record_type("MyStore.Dto.Shop.SetCurrentCartQuery", [], SetCurrentCartQuery, () => [["setCurrent", bool_type]]);
}

