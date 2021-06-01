import { Record, Union } from "./.fable/fable-library.3.1.11/Types.js";
import { obj_type, record_type, array_type, union_type, class_type, string_type, int32_type } from "./.fable/fable-library.3.1.11/Reflection.js";
import { ProductsModel$reflection } from "./MyStore.Domain/Dto/Shop.js";
import { Product_ToDomain_Z6B097A11, Product$reflection } from "./MyStore.Domain/Shop.js";
import { map } from "./.fable/fable-library.3.1.11/Array.js";
import { extra, acceptJson, baseUrl, pageToOffset, itemsPerPage, offsetToPage } from "./Extensions.js";
import { Cmd_OfPromise_either, Cmd_ofSub, Cmd_none } from "./.fable/Fable.Elmish.3.1.0/cmd.fs.js";
import { PromiseBuilder__Delay_62FBFDE1, PromiseBuilder__Run_212F1D4B } from "./.fable/Fable.Promise.2.0.0/Promise.fs.js";
import { interpolate, toText } from "./.fable/fable-library.3.1.11/String.js";
import { promise } from "./.fable/Fable.Promise.2.0.0/PromiseImpl.fs.js";
import { Fetch_get_5760677E } from "./.fable/Thoth.Fetch.2.0.0/Fetch.fs.js";
import { CaseStrategy } from "./.fable/Thoth.Json.4.0.0/Types.fs.js";
import { uncurry } from "./.fable/fable-library.3.1.11/Util.js";
import { RouterModule_nav } from "./.fable/Feliz.Router.3.8.0/Router.fs.js";
import { singleton } from "./.fable/fable-library.3.1.11/List.js";
import { some } from "./.fable/fable-library.3.1.11/Option.js";
import { useFeliz_React__React_useElmish_Static_645B1FB7 } from "./.fable/Feliz.UseElmish.1.5.1/UseElmish.fs.js";
import { map as map_1, singleton as singleton_1, append, delay, toList } from "./.fable/fable-library.3.1.11/Seq.js";
import { createElement } from "react";
import { Interop_reactApi } from "./.fable/Feliz.1.45.0/Interop.fs.js";

export class Msg extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["SetPage", "Fetched", "Failed"];
    }
}

export function Msg$reflection() {
    return union_type("MyStore.Client.Products.Msg", [], Msg, () => [[["Item", int32_type]], [["Item1", ProductsModel$reflection()], ["Item2", string_type]], [["Item", class_type("System.Exception")]]]);
}

export class State extends Record {
    constructor(Products, Page) {
        super();
        this.Products = Products;
        this.Page = (Page | 0);
    }
}

export function State$reflection() {
    return record_type("MyStore.Client.Products.State", [], State, () => [["Products", array_type(Product$reflection())], ["Page", int32_type]]);
}

export function init(productsModel, unitVar1) {
    return [new State(map((arg00) => Product_ToDomain_Z6B097A11(arg00), productsModel.products), offsetToPage(productsModel.query.offset)), Cmd_none()];
}

export function getProducts(page) {
    return PromiseBuilder__Run_212F1D4B(promise, PromiseBuilder__Delay_62FBFDE1(promise, () => {
        const url = toText(interpolate("/Shop/Product?count=%i%P()\u0026offset=%i%P()", [itemsPerPage, pageToOffset(page)]));
        return Fetch_get_5760677E(baseUrl() + url, void 0, void 0, acceptJson, new CaseStrategy(1), extra, uncurry(2, void 0), {
            ResolveType: ProductsModel$reflection,
        }, {
            ResolveType: () => obj_type,
        }).then(((_arg1) => (Promise.resolve((new Msg(1, _arg1, url))))));
    }));
}

export function update(msg, state) {
    switch (msg.tag) {
        case 1: {
            return [new State(map((arg00) => Product_ToDomain_Z6B097A11(arg00), msg.fields[0].products), state.Page), Cmd_ofSub((_arg138) => {
                RouterModule_nav(singleton(msg.fields[1]), 1, 2);
            })];
        }
        case 2: {
            console.error(some(msg.fields[0]));
            return [state, Cmd_none()];
        }
        default: {
            const page = msg.fields[0] | 0;
            return [new State(state.Products, page), Cmd_OfPromise_either((page_1) => getProducts(page_1), page, (x) => x, (arg0) => (new Msg(2, arg0)))];
        }
    }
}

export function Products(productsModel) {
    const patternInput = useFeliz_React__React_useElmish_Static_645B1FB7(() => init(productsModel, void 0), (msg, state) => update(msg, state), []);
    const state_1 = patternInput[0];
    const dispatch = patternInput[1];
    const children = toList(delay(() => append(singleton_1(createElement("button", {
        children: "+",
        onClick: (_arg1) => {
            dispatch(new Msg(0, state_1.Page + 1));
        },
    })), delay(() => {
        let value_3;
        return append(singleton_1((value_3 = toText(interpolate("Page: %i%P()", [state_1.Page])), createElement("p", {
            children: [value_3],
        }))), delay(() => append(singleton_1(createElement("button", {
            children: "-",
            onClick: (_arg2) => {
                dispatch(new Msg(0, state_1.Page - 1));
            },
        })), delay(() => map_1((i) => {
            let value_7;
            return createElement("a", {
                children: Interop_reactApi.Children.toArray([(value_7 = toText(interpolate("%A%P()", [i])), createElement("p", {
                    children: [value_7],
                }))]),
                href: toText(interpolate("/Shop/Product/%i%P()", [i.ProductId])),
            });
        }, state_1.Products)))));
    }))));
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    });
}

