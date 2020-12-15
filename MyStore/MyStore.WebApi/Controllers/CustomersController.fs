namespace MyStore.WebApi.Controllers

open System.Collections.Generic
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open MyStore.Data
open MyStore.Data
open MyStore.Data.Entity
open MyStore.Data

type CustomerDTO =
    { FirstName: string
      LastName: string
      Honorific: string
      Email: string
      Password: string }
   

[<ApiController>]
[<Route("[controller]")>]
type CustomersController(logger: ILogger<CustomersController>, context: Context) =
    inherit ControllerBase()

    [<HttpGet("getOffset/{ntake}/{nskip}")>]
    member this.GetOffset(ntake, nskip) =
        ActionResult<IEnumerable<Customer>>
            (query {
                for i in context.Customers do
                    select i
                    take ntake
                    skip nskip
             })


    [<HttpGet("getById/{id}")>]
    member this.GetById(id) =
        if (query {
                for i in context.Customers do
                    exists (i.CustomerId = id)
            }) then
            ActionResult<Customer>
                (query {
                    for i in context.Customers do
                        where (i.CustomerId = id)
                        exactlyOne
                 })
        else
            ActionResult<Customer>(base.NotFound())

    [<HttpDelete("deleteById/{id}")>]
    member this.DeleteById(id) =
        if (query {
                for i in context.Customers do
                    exists (i.CustomerId = id)
            }) then
            context.Customers.Remove
                (query {
                    for i in context.Customers do
                        where (i.CustomerId = id)
                        exactlyOne
                 })
            |> ignore

            context.SaveChanges() |> ignore

            ActionResult<Customer>(base.NoContent())
        else
            ActionResult<Customer>(base.NotFound())


    [<HttpPost("add")>]
    member this.Add([<FromBody>] customerDto) =
        let customerEntity = Customer()
        customerEntity.FirstName <- customerDto.FirstName
        customerEntity.LastName <- customerDto.LastName
        customerEntity.Honorific <- customerDto.Honorific
        customerEntity.Email <- customerDto.Email
        customerEntity.PasswordSalt <- Crypto.GenerateSaltForPassword()
        customerEntity.PasswordHash <- Crypto.ComputePasswordHash(customerDto.Password, customerEntity.PasswordSalt)

        context.Customers.Add(customerEntity) |> ignore
        context.SaveChanges() |> ignore
        
        ActionResult<Customer>(base.Created("getById", customerDto))
