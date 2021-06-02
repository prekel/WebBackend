namespace MyStore.Domain.Shop

open System
open FSharp.UMX

open MyStore.Domain.SimpleTypes
open MyStore.Dto.Shop

type Product =
    { ProductId: ProductId
      Name: string
      Description: string
      Price: decimal }
    static member ToDomain(dto: ProductDto) =
        { ProductId = %dto.productId
          Name = dto.name
          Description = dto.description
          Price = decimal dto.price }

    member this.FromDomain() =
        { ProductDto.productId = %this.ProductId
          name = this.Name
          description = this.Description
          price = double this.Price }

type Customer =
    { CustomerId: CustomerId
      FirstName: string
      LastName: string option
      Honorific: string option
      Email: Email
      CurrentCartId: CartId option }
    static member ToDomain(dto: CustomerDto) =
        { CustomerId = %dto.customerId
          FirstName = dto.firstName
          LastName = dto.lastName |> Option.ofObj
          Honorific = dto.honorific |> Option.ofObj
          Email = %dto.email
          CurrentCartId =
              dto.currentCartId
              |> Option.ofNullable
              |> Option.map (~%) }

    member this.FromDomain() =
        { CustomerDto.customerId = %this.CustomerId
          firstName = this.FirstName
          lastName = this.LastName |> Option.toObj
          honorific = this.Honorific |> Option.toObj
          email = %this.Email
          currentCartId =
              this.CurrentCartId
              |> Option.map (~%)
              |> Option.toNullable }

type Cart =
    { CartId: CartId
      IsPublic: bool
      OwnerCustomerId: CustomerId option }
    static member ToDomain(dto: CartDto) =
        { CartId = %dto.cartId
          IsPublic = dto.isPublic
          OwnerCustomerId =
              dto.ownerCustomerId
              |> Option.ofNullable
              |> Option.map (~%) }

    member this.FromDomain() =
        { CartDto.cartId = %this.CartId
          isPublic = this.IsPublic
          ownerCustomerId =
              this.OwnerCustomerId
              |> Option.map (~%)
              |> Option.toNullable }

type Order =
    { OrderId: OrderId
      CustomerId: CustomerId
      CreateTimeOffset: DateTimeOffset }

    static member ToDomain(dto: OrderDto) =
        { OrderId = %dto.orderId
          CustomerId = %dto.customerId
          CreateTimeOffset = dto.createTimeOffset }

    member this.FromDomain() =
        { OrderDto.orderId = %this.OrderId
          customerId = %this.CustomerId
          createTimeOffset = this.CreateTimeOffset }
