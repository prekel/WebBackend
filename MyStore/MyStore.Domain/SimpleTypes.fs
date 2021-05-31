namespace MyStore.Domain.SimpleTypes

open System
open System.Net.Mail
open FSharp.UMX
open FsToolkit.ErrorHandling

[<Measure>]
type private productId


[<Measure>]
type private rouble

[<Measure>]
type private customerId

[<Measure>]
type private cartId

type CartId = int<cartId>

type CustomerId = int<customerId>

type ProductId = int<productId>

type ProductPrice = decimal<rouble>

type String50 = private String50 of string

type EmailAddress = private EmailAddress of MailAddress
