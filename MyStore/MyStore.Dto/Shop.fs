namespace MyStore.Dto.Shop

open System

type ProductDto =
    { productId: int
      name: string
      description: string
      price: decimal }

type CustomerDto =
    { customerId: int
      firstName: string
      lastName: string option
      honorific: string option
      email: string
      userId: string option
      currentCartId: int option }

type CartDto =
    { cartId: int
      isPublic: bool
      ownerCustomerId: int option }

type OrderDto =
    { orderId: int
      customerId: int
      createTimeOffset: DateTimeOffset }

type OrderedProductDto =
    { productId: int
      orderId: int
      orderedPrice: decimal }


type CartModel =
    { cart: CartDto
      products: ProductDto array }

type OrderModel =
    { order: OrderDto
      orderedProducts: OrderedProductDto array }

type ProductsModel = { products: ProductDto array }
