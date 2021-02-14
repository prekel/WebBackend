namespace Products

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Config
open Saturn

module Controller =

    let indexAction (ctx: HttpContext) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Database.getAll cnf.Context

            match result with
            | Ok result -> return! Controller.renderHtml ctx (Views.index ctx (List.ofSeq result))
            | Error ex -> return raise ex
        }

    let showAction (ctx: HttpContext) (id: int) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Database.getById cnf.Context id

            match result with
            | Ok (Some result) -> return! Controller.renderHtml ctx (Views.show ctx result)
            | Ok None -> return! Controller.renderHtml ctx (NotFound.layout)
            | Error ex -> return raise ex
        }

    let addAction (ctx: HttpContext) =
        task { return! Controller.renderHtml ctx (Views.add ctx None Map.empty) }

    let editAction (ctx: HttpContext) (id: int) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Database.getById cnf.Context id

            match result with
            | Ok (Some result) -> return! Controller.renderHtml ctx (Views.edit ctx result Map.empty)
            | Ok None -> return! Controller.renderHtml ctx (NotFound.layout)
            | Error ex -> return raise ex
        }

    let createAction (ctx: HttpContext) =
        task {
            let! input = Controller.getModel<Product> ctx
            let validateResult = Validation.validate input

            if validateResult.IsEmpty then

                let cnf = Controller.getConfig ctx
                let! result = Database.insert cnf.Context input

                match result with
                | Ok _ -> return! Controller.redirect ctx (Links.index ctx)
                | Error ex -> return raise ex
            else
                return! Controller.renderHtml ctx (Views.add ctx (Some input) validateResult)
        }

    let updateAction (ctx: HttpContext) (id: int) =
        task {
            let! input = Controller.getModel<Product> ctx
            let validateResult = Validation.validate input

            if validateResult.IsEmpty then
                let cnf = Controller.getConfig ctx
                let! result = Database.update cnf.Context input

                match result with
                | Ok _ -> return! Controller.redirect ctx (Links.index ctx)
                | Error ex -> return raise ex
            else
                return! Controller.renderHtml ctx (Views.edit ctx input validateResult)
        }

    let deleteAction (ctx: HttpContext) (id: int) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Database.delete cnf.Context id

            match result with
            | Ok _ -> return! Controller.redirect ctx (Links.index ctx)
            | Error ex -> return raise ex
        }

    let resource =
        controller {
            index indexAction
            show showAction
            add addAction
            edit editAction
            create createAction
            update updateAction
            delete deleteAction
        }
