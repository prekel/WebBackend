module MyStore.Saturn.Customers.Repository

open System.Net.Mail
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive
open Npgsql
open FSharp.UMX

open MyStore.Saturn.Database
open MyStore.Saturn.Domain

type CustomerDto =
    { CustomerId: int
      FirstName: string
      LastName: string
      Honorific: string
      Email: string
      CurrentCartId: int }

    static member FromDomain(customer: Customer): CustomerDto =
        { CustomerId = %customer.CustomerId
          FirstName = %customer.FirstName
          LastName =
              match customer.LastName with
              | Some a -> %a
              | None -> null
          Honorific = %customer.Honorific
          Email = customer.Email.Address
          CurrentCartId =
              match customer.CurrentCart with
              | LoadedCart cart -> %cart.CartId
              | NotLoadedCart cartId -> %cartId }

    member this.ToDomain(): Customer =
        { CustomerId = %this.CustomerId
          FirstName = %this.FirstName
          LastName = if isNull <| this.LastName then None else Some %this.LastName
          Honorific = %this.Honorific
          Email = MailAddress(this.Email)
          CurrentCart = NotLoadedCart %this.CurrentCartId
          Orders = NotLoadedOrders }

let getById connectionString (id: int): Task<Result<Customer option, exn>> =
    task {
        use connection = new NpgsqlConnection(connectionString)

        let! res =
            querySingle
                connection
                """SELECT "CustomerId", "FirstName", "LastName", "Honorific", "Email", "CurrentCartId" FROM "Customers" WHERE "CustomerId"=@CustomerId """
                (Some <| dict [ "CustomerId" => id ])

        return
            res
            |> Result.map (fun (dto: CustomerDto option) -> dto |> Option.map (fun d -> d.ToDomain()))
    }
