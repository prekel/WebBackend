import { Record, Union } from "./.fable/fable-library.3.1.11/Types.js";
import { obj_type, record_type, bool_type, union_type, class_type, string_type, int32_type } from "./.fable/fable-library.3.1.11/Reflection.js";
import { ProductModel$reflection } from "./MyStore.Domain/Dto/Shop.js";
import { Product_ToDomain_Z6B097A11, Product$reflection } from "./MyStore.Domain/Shop.js";
import { Cmd_OfPromise_either, Cmd_ofSub, Cmd_none } from "./.fable/Fable.Elmish.3.1.0/cmd.fs.js";
import { PromiseBuilder__Delay_62FBFDE1, PromiseBuilder__Run_212F1D4B } from "./.fable/Fable.Promise.2.1.0/Promise.fs.js";
import { interpolate, toText } from "./.fable/fable-library.3.1.11/String.js";
import { promise } from "./.fable/Fable.Promise.2.1.0/PromiseImpl.fs.js";
import { Fetch_get_5760677E } from "./.fable/Thoth.Fetch.2.0.0/Fetch.fs.js";
import { extra, acceptJson, baseUrl } from "./Extensions.js";
import { CaseStrategy } from "./.fable/Thoth.Json.4.0.0/Types.fs.js";
import { uncurry } from "./.fable/fable-library.3.1.11/Util.js";
import { RouterModule_nav } from "./.fable/Feliz.Router.3.8.0/Router.fs.js";
import { ofArray, singleton } from "./.fable/fable-library.3.1.11/List.js";
import { some } from "./.fable/fable-library.3.1.11/Option.js";
import { useFeliz_React__React_useElmish_Static_645B1FB7 } from "./.fable/Feliz.UseElmish.1.5.1/UseElmish.fs.js";
import { useFeliz_React__React_useState_Static_1505 } from "./.fable/Feliz.1.45.0/React.fs.js";
import { createElement } from "react";
import { parse } from "./.fable/fable-library.3.1.11/Int32.js";
import { Interop_reactApi } from "./.fable/Feliz.1.45.0/Interop.fs.js";

export class Msg extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["Fetch", "Fetched", "Failed", "ToCart"];
    }
}

export function Msg$reflection() {
    return union_type("MyStore.Client.Product.Msg", [], Msg, () => [[["Item", int32_type]], [["Item1", ProductModel$reflection()], ["Item2", string_type]], [["Item", class_type("System.Exception")]], []]);
}

export class State extends Record {
    constructor(Product, IsInCart) {
        super();
        this.Product = Product;
        this.IsInCart = IsInCart;
    }
}

export function State$reflection() {
    return record_type("MyStore.Client.Product.State", [], State, () => [["Product", Product$reflection()], ["IsInCart", bool_type]]);
}

export function init(productModel, unitVar1) {
    return [new State(Product_ToDomain_Z6B097A11(productModel.product), productModel.isInCart), Cmd_none()];
}

export function getProductById(id) {
    return PromiseBuilder__Run_212F1D4B(promise, PromiseBuilder__Delay_62FBFDE1(promise, () => {
        const url = toText(interpolate("/Shop/Product/%i%P()", [id]));
        return Fetch_get_5760677E(baseUrl() + url, void 0, void 0, acceptJson, new CaseStrategy(1), extra, uncurry(2, void 0), {
            ResolveType: ProductModel$reflection,
        }, {
            ResolveType: () => obj_type,
        }).then(((_arg1) => (Promise.resolve((new Msg(1, _arg1, url))))));
    }));
}

export function update(msg, state) {
    switch (msg.tag) {
        case 1: {
            const productModel = msg.fields[0];
            return [new State(Product_ToDomain_Z6B097A11(productModel.product), productModel.isInCart), Cmd_ofSub((_arg138) => {
                RouterModule_nav(singleton(msg.fields[1]), 1, 2);
            })];
        }
        case 2: {
            console.error(some(msg.fields[0]));
            return [state, Cmd_none()];
        }
        case 3: {
            return [state, Cmd_none()];
        }
        default: {
            return [state, Cmd_OfPromise_either((id) => getProductById(id), msg.fields[0], (x_3) => x_3, (arg0) => (new Msg(2, arg0)))];
        }
    }
}

export function Product(productModel) {
    let value_7;
    const patternInput = useFeliz_React__React_useElmish_Static_645B1FB7(() => init(productModel, void 0), (msg, state) => update(msg, state), []);
    const state_1 = patternInput[0];
    const dispatch = patternInput[1];
    const patternInput_1 = useFeliz_React__React_useState_Static_1505(productModel.product.productId);
    const children = ofArray([createElement("input", {
        onChange: (ev) => {
            patternInput_1[1](parse(ev.target.value, 511, false, 32));
        },
    }), createElement("button", {
        children: "GetById",
        onClick: (_arg1) => {
            dispatch(new Msg(0, patternInput_1[0]));
        },
    }), createElement("button", {
        children: state_1.IsInCart ? "Remove from cart" : "Add to cart",
        onClick: (_arg2) => {
            dispatch(new Msg(3));
        },
    }), (value_7 = toText(interpolate("%A%P()", [state_1])), createElement("p", {
        children: [value_7],
    }))]);
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    });
}
