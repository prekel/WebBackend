module Models

open System

type Customer =
    { CustomerId: int
      FirstName: string
      LastName: string option
      Honorific: string
      Email: string
      CurrentCartId: int option }

type Cart =
    { CartId: int
      IsPublic: bool
      OwnerCustomerId: int option }

type Order =
    { OrderId: int
      CustomerId: int
      CreateTimeOffset: DateTimeOffset }

type Product =
    { ProductId: int
      Name: string
      Description: string
      Price: decimal }

type OrderedProduct =
    { ProductId: int
      OrderId: int
      OrderedPrice: decimal }
