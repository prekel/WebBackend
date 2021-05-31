namespace MyStore.Dto.Shop

open System
open System.ComponentModel.DataAnnotations

type ProductDto =
    { [<Required>]
      ProductId: int
      [<Required>]
      Name: string
      [<Required>]
      Description: string
      [<Required>]
      Price: decimal }

type CustomerDto =
    { [<Required>]
      CustomerId: int
      [<Required>]
      FirstName: string
      LastName: string
      Honorific: string
      [<Required>]
      Email: string
      UserId: string
      CurrentCartId: Nullable<int> }

type CartDto =
    { [<Required>]
      CartId: int
      [<Required>]
      IsPublic: bool
      OwnerCustomerId: Nullable<int> }

type OrderDto =
    { [<Required>]
      OrderId: int
      [<Required>]
      CustomerId: int
      [<Required>]
      CreateTimeOffset: DateTimeOffset }

type OrderedProductDto =
    { [<Required>]
      ProductId: int
      [<Required>]
      OrderId: int
      [<Required>]
      OrderedPrice: decimal }


type CartModel =
    { [<Required>]
      Cart: CartDto
      [<Required>]
      Products: ProductDto array }

type OrderModel =
    { [<Required>]
      Order: OrderDto
      [<Required>]
      OrderedProducts: OrderedProductDto array }

type ProductsModel =
    { [<Required>]
      Products: ProductDto array }
