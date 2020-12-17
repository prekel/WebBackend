namespace MyStore.WebApi.Controllers

open System
open System.Collections.Generic
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open MyStore.Data
open MyStore.Data
open MyStore.Data.Entity
open MyStore.Data
open MyStore.WebApi.Utils
open MyStore.Shared.Dtos
open MyStore.WebApi.DtoConvert


[<ApiController>]
[<Route("[controller]")>]
type CustomersController(logger: ILogger<CustomersController>, context: Context) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.GetOffset([<FromQuery>] start: Nullable<int>, [<FromQuery>] limit: Nullable<int>) =
        let nskip, ntake =
            nullableLimitStartToSkipTake (start, limit)

        ActionResult<IEnumerable<CustomerDto>>
            (query {
                for i in context.Customers do
                    sortBy i.CustomerId
                    select i
                    skip nskip
                    take ntake
             }
             |> Seq.map customerDtoFromEntity)


    [<HttpGet("{id}")>]
    member this.GetById(id) =
        if (query {
                for i in context.Customers do
                    exists (i.CustomerId = id)
            }) then
            ActionResult<CustomerDto>
                (query {
                    for i in context.Customers do
                        where (i.CustomerId = id)
                        exactlyOne
                 } |> customerDtoFromEntity)
        else
            ActionResult<CustomerDto>(base.NotFound())

    [<HttpDelete("{id}")>]
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

            ActionResult<CustomerDto>(base.NoContent())
        else
            ActionResult<CustomerDto>(base.NotFound())

    [<HttpPut("{id}")>]
    member this.Update(id, [<FromBody>] customerDto) =
        if (query {
                for i in context.Customers do
                    exists (i.CustomerId = id)
            }) then

            let customerEntity =
                query {
                    for i in context.Customers do
                        where (i.CustomerId = id)
                        exactlyOne
                }

            customerEntity.FirstName <- customerDto.FirstName
            customerEntity.LastName <- customerDto.LastName
            customerEntity.Honorific <- customerDto.Honorific
            customerEntity.Email <- customerDto.Email

            context.SaveChanges() |> ignore

            ActionResult<Customer>(base.NoContent())
        else
            ActionResult<Customer>(base.NotFound())

    [<HttpPost>]
    member this.Add([<FromBody>] customerDto) =
        let customerEntity = Customer()
        customerEntity.FirstName <- customerDto.FirstName
        customerEntity.LastName <- customerDto.LastName
        customerEntity.Honorific <- customerDto.Honorific
        customerEntity.Email <- customerDto.Email
        //customerEntity.PasswordSalt <- Crypto.GenerateSaltForPassword()
        //customerEntity.PasswordHash <- Crypto.ComputePasswordHash(customerDto.Password, customerEntity.PasswordSalt)

        context.Customers.Add(customerEntity) |> ignore
        context.SaveChanges() |> ignore

        ActionResult<Customer>(base.Created($"customers/{customerEntity.CustomerId}", customerEntity))
