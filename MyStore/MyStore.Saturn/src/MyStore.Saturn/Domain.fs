namespace MyStore.Saturn.Domain

open System.Net.Mail
open FSharp.UMX


[<Measure>]
type private productId

type ProductId = int<productId>

[<Measure>]
type private productName

type ProductName = string<productName>

[<Measure>]
type private productDescription

type ProductDescription = string<productDescription>

[<Measure>]
type private rouble

type ProductPrice = decimal<rouble>

type ProductRating =
    | Rating01
    | Rating02
    | Rating03
    | Rating04
    | Rating05
    | Rating06
    | Rating07
    | Rating08
    | Rating09
    | Rating10

[<Measure>]
type private customerId

type CustomerId = int<customerId>

[<Measure>]
type private firstName

type FirstName = string<firstName>

[<Measure>]
type private lastName

type CustomerLastName = string<lastName> option

[<Measure>]
type private honorific

type Honorific = string<honorific>

[<Measure>]
type private cartId

type CartId = int<cartId>


[<Measure>]
type private orderId

type OrderId = int<orderId>

[<Measure>]
type private orderCreateDateTime

type OrderCreateDateTime = DateTimeOffset<orderCreateDateTime>


type Product =
    { ProductId: ProductId
      Name: ProductName
      Description: ProductDescription
      Price: ProductPrice
      Rating: ProductRating }

type OrderedProduct =
    { Product: Product
      OrderId: OrderId
      OrderedPrice: ProductPrice }

type Cart =
    { CartId: CartId
      IsPublic: bool
      OwnerId: CustomerId
      Products: Product list }

type CartLoad =
    | LoadedCart of Cart
    | NotLoadedCart of CartId

type Order =
    { OrderId: OrderId
      CustomerId: CustomerId
      CreateDateTime: OrderCreateDateTime
      OrderedProducts: OrderedProduct list }

type OrdersLoad =
    | LoadedOrders of Order list
    | NotLoadedOrders

type Customer =
    { CustomerId: CustomerId
      FirstName: FirstName
      LastName: CustomerLastName
      Honorific: Honorific
      Email: MailAddress
      CurrentCart: CartLoad
      Orders: OrdersLoad }
