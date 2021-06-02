namespace MyStore.Domain.SimpleTypes

open System

open FSharp.UMX

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

[<Measure>]
type private userId

type UserId = string<userId>

[<Measure>]
type private email

type Email = string<email>


[<Measure>]
type private operatorId

type OperatorId = int<operatorId>

[<Measure>]
type private answerId

type AnswerId = int<answerId>

[<Measure>]
type private ticketId

type TicketId = int<ticketId>

[<Measure>]
type private questionId

type QuestionId = int<questionId>


[<Measure>]
type private orderId;
type OrderId = int<orderId>
