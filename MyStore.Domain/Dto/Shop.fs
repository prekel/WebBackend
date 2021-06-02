namespace MyStore.Dto.Shop

open System
#if FABLE_COMPILER
open Fable.System.ComponentModel.Annotations
#else
open System.ComponentModel.DataAnnotations
#endif

type ProductDto =
    { [<Required>]
      productId: int
      [<Required>]
      name: string
      [<Required>]
      description: string
      [<Required>]
      price: double }

type CustomerDto =
    { [<Required>]
      customerId: int
      [<Required>]
      firstName: string
      lastName: string
      honorific: string
      [<Required>]
      email: string
      currentCartId: Nullable<int> }

type CartDto =
    { [<Required>]
      cartId: int
      [<Required>]
      isPublic: bool
      ownerCustomerId: Nullable<int> }

type OrderDto =
    { [<Required>]
      orderId: int
      [<Required>]
      customerId: int
      [<Required>]
      createTimeOffset: DateTimeOffset }

type OrderedProductDto =
    { [<Required>]
      productId: int
      [<Required>]
      orderId: int
      [<Required>]
      orderedPrice: double }


type CartModel =
    { [<Required>]
      cart: CartDto
      [<Required>]
      products: ProductDto array
      [<Required>]
      isCurrent: bool }

type OrderModel =
    { [<Required>]
      order: OrderDto
      [<Required>]
      orderedProducts: OrderedProductDto array }


type CartsQuery =
    { [<Required>]
      isPublic: bool
      [<Required>]
      count: int
      [<Required>]
      offset: int }

type CartsModel =
    { [<Required>]
      carts: CartDto array
      [<Required>]
      query: CartsQuery }

type SetCurrentCartQuery = { setCurrent: bool }

type ProductModel =
    { [<Required>]
      product: ProductDto
      [<Required>]
      isInCart: bool
      [<Required>]
      isLoggedIn: bool }

type ProductsQuery =
    { [<Required>]
      count: int
      [<Required>]
      offset: int }

type ProductsModel =
    { [<Required>]
      products: ProductDto array
      [<Required>]
      query: ProductsQuery }
