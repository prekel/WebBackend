namespace MyStore.WebApi.Repository

open System.Linq
open Microsoft.EntityFrameworkCore
open MyStore.Data
open MyStore.Data.Entity

module Customers =
    let exists (context: Context) id =
        query {
            for i in context.Customers do
                exists (i.CustomerId = id)
        }

    let exactlyOne (context: Context) id =
        query {
            for i in context.Customers do
                where (i.CustomerId = id)
                exactlyOne
        }

    let skipTake (context: Context) (nskip, ntake) =
        query {
            for i in context.Customers do
                sortBy i.CustomerId
                select i
                skip nskip
                take ntake
        }


module Carts =
    let exists (context: Context) id =
        query {
            for i in context.Carts do
                exists (i.CartId = id)
        }

    let exactlyOne (context: Context) id =
        query {
            for i in context.Carts do
                where (i.CartId = id)
                exactlyOne
        }

    let exactlyOneIncludeProducts (context: Context) id =
        query {
            for i in context.Carts.Include(fun j -> j.Products) do
                where (i.CartId = id)
                exactlyOne
        }

    let skipTake (context: Context) (nskip, ntake) =
        query {
            for i in context.Carts do
                sortBy i.CartId
                select i
                skip nskip
                take ntake
        }


module Products =
    let exists (context: Context) id =
        query {
            for i in context.Products do
                exists (i.ProductId = id)
        }

    let exactlyOne (context: Context) id =
        query {
            for i in context.Products do
                where (i.ProductId = id)
                exactlyOne
        }

    let skipTake (context: Context) (nskip, ntake) =
        query {
            for i in context.Products do
                sortBy i.ProductId
                select i
                skip nskip
                take ntake
        }

module Orders =
    let exists (context: Context) id =
        query {
            for i in context.Orders do
                exists (i.OrderId = id)
        }

    let exactlyOne (context: Context) id =
        query {
            for i in context.Orders do
                where (i.OrderId = id)
                exactlyOne
        }

    let exactlyOneIncludeOrderedProducts (context: Context) id =
        query {
            for i in context.Orders.Include(fun i -> i.OrderedProducts) do
                where (i.OrderId = id)
                exactlyOne
        }

    let skipTake (context: Context) (nskip, ntake) =
        query {
            for i in context.Orders do
                sortBy i.OrderId
                select i
                skip nskip
                take ntake
        }


module OrderedProducts =
    let exists (context: Context) orderId productId =
        query {
            for i in context.OrderedProducts do
                exists (i.ProductId = productId && i.OrderId = orderId)
        }

    let exactlyOne (context: Context) orderId productId =
        query {
            for i in context.OrderedProducts do
                where (i.ProductId = productId && i.OrderId = orderId)
                exactlyOne
        }
