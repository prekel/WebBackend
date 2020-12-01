open System
open System.Text
open System.Linq
open FSharp.Data.Sql

[<Literal>]
let resolutionPath = __SOURCE_DIRECTORY__ + "/libraries"

[<Literal>]
let connectionString =
    "Host=localhost;Database=postgres;Username=postgres;Password=qwerty123"

type Sql = SqlDataProvider<Common.DatabaseProviderTypes.POSTGRESQL, connectionString, ResolutionPath=resolutionPath>

let ctx = Sql.GetDataContext()

[<EntryPoint>]
let main argv =
    Console.OutputEncoding <- Encoding.UTF8
    FSharp.Data.Sql.Common.QueryEvents.SqlQueryEvent |> Event.add (printfn "\nExecuting SQL: %O")

    let queryForIndex name =
        let q1 =
            query {
                for question in ctx.Public.SupportQuestions do
                    join ticket in ctx.Public.SupportTickets on (question.SupportTicketId = ticket.SupportTicketId)
                    join customer in ctx.Public.Customers on (ticket.CustomerId = customer.CustomerId)
                    where (customer.FirstName = name)

                    select
                        {| SupportTicketId = question.SupportTicketId
                           IsQuestion = true
                           SendTimestamp = question.SendTimestamp
                           FirstName = customer.FirstName
                           LastName = customer.LastName
                           Text = question.Text |}

            }
            |> Seq.toList

        let q2 =
            query {
                for answer in ctx.Public.SupportAnswers do
                    join op in ctx.Public.SupportOperators on (answer.SupportOperatorId = op.SupportOperatorId)
                    where (op.FirstName = name)
                    sortBy (answer.SupportTicketId)
                    sortBy (answer.SendTimestamp)

                    select
                        {| SupportTicketId = answer.SupportTicketId
                           IsQuestion = false
                           SendTimestamp = answer.SendTimestamp
                           FirstName = op.FirstName
                           LastName = op.LastName
                           Text = answer.Text |}

            }
            |> Seq.toList

        q1 @ q2

    let qwe = queryForIndex "Юлия" |> Seq.toList
    printfn "%d" (qwe.Count())

    let y =
        qwe
        |> List.sortBy (fun arg -> arg.SendTimestamp)
        |> List.sortBy (fun arg -> arg.SupportTicketId)
        |> List.take 10

    y |> List.iter (printfn "%A")

    query {
        for customer in ctx.Public.Customers do
            count
    }
    |> printf "%d\n"

    let customer34 =
        query {
            for customer in ctx.Public.Customers do
                where (customer.CustomerId = 34)
                select $"{customer.FirstName} {customer.LastName}"
        }
        |> Seq.head

    let annas =
        query {
            for customer in ctx.Public.Customers do
                where (customer.FirstName = "Анна")
                select customer
                take 10
        }

    printf "%s\n" customer34

    annas
    |> Seq.toList
    |> List.map (fun customer -> printf "%s\n" $"{customer.FirstName} {customer.LastName}")
    |> ignore

    0