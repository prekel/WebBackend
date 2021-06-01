import { Record, Union } from "./.fable/fable-library.3.1.11/Types.js";
import { obj_type, record_type, array_type, union_type, class_type, int32_type } from "./.fable/fable-library.3.1.11/Reflection.js";
import { CartModel$reflection } from "./MyStore.Dto/Shop.js";
import { Product_ToDomain_Z6B097A11, Cart_ToDomain_268DEFC0, Product$reflection, Cart$reflection } from "./MyStore.Domain/Shop.js";
import { map } from "./.fable/fable-library.3.1.11/Array.js";
import { Cmd_OfPromise_either, Cmd_ofSub, Cmd_none } from "./.fable/Fable.Elmish.3.1.0/cmd.fs.js";
import { PromiseBuilder__Delay_62FBFDE1, PromiseBuilder__Run_212F1D4B } from "./.fable/Fable.Promise.2.0.0/Promise.fs.js";
import { interpolate, toText } from "./.fable/fable-library.3.1.11/String.js";
import { extra, acceptJson, baseUrl } from "./Extensions.js";
import { promise } from "./.fable/Fable.Promise.2.0.0/PromiseImpl.fs.js";
import { Fetch_get_5760677E } from "./.fable/Thoth.Fetch.2.0.0/Fetch.fs.js";
import { CaseStrategy } from "./.fable/Thoth.Json.4.0.0/Types.fs.js";
import { uncurry } from "./.fable/fable-library.3.1.11/Util.js";
import { RouterModule_nav } from "./.fable/Feliz.Router.3.8.0/Router.fs.js";
import { singleton } from "./.fable/fable-library.3.1.11/List.js";
import { value as value_4, some } from "./.fable/fable-library.3.1.11/Option.js";
import { useFeliz_React__React_useElmish_Static_645B1FB7 } from "./.fable/Feliz.UseElmish.1.5.1/UseElmish.fs.js";
import { useReact_useInputRef } from "./.fable/Feliz.1.45.0/React.fs.js";
import { map as map_1, singleton as singleton_1, append, delay, toList } from "./.fable/fable-library.3.1.11/Seq.js";
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
        return ["Fetch", "Fetched", "Failed"];
    }
}

export function Msg$reflection() {
    return union_type("MyStore.Client.Cart.Msg", [], Msg, () => [[["Item", int32_type]], [["Item", CartModel$reflection()]], [["Item", class_type("System.Exception")]]]);
}

export class State extends Record {
    constructor(Cart, Products) {
        super();
        this.Cart = Cart;
        this.Products = Products;
    }
}

export function State$reflection() {
    return record_type("MyStore.Client.Cart.State", [], State, () => [["Cart", Cart$reflection()], ["Products", array_type(Product$reflection())]]);
}

export function modelToDomain(model) {
    return [Cart_ToDomain_268DEFC0(model.cart), map((arg00_1) => Product_ToDomain_Z6B097A11(arg00_1), model.products)];
}

export function init(cartModel, unitVar1) {
    const patternInput = modelToDomain(cartModel);
    return [new State(patternInput[0], patternInput[1]), Cmd_none()];
}

export function getCartById(id) {
    return PromiseBuilder__Run_212F1D4B(promise, PromiseBuilder__Delay_62FBFDE1(promise, () => {
        const url = toText(interpolate("%s%P()/Shop/Cart/%i%P()", [baseUrl(), id]));
        return Fetch_get_5760677E(url, void 0, void 0, acceptJson, new CaseStrategy(1), extra, uncurry(2, void 0), {
            ResolveType: CartModel$reflection,
        }, {
            ResolveType: () => obj_type,
        }).then(((_arg1) => (Promise.resolve((new Msg(1, _arg1))))));
    }));
}

export function update(msg, state) {
    let fullPath;
    switch (msg.tag) {
        case 1: {
            const patternInput = modelToDomain(msg.fields[0]);
            const cart = patternInput[0];
            return [new State(cart, patternInput[1]), (fullPath = toText(interpolate("/Shop/Cart/%i%P()", [cart.CartId])), Cmd_ofSub((_arg138) => {
                RouterModule_nav(singleton(fullPath), 1, 2);
            }))];
        }
        case 2: {
            console.error(some(msg.fields[0]));
            return [state, Cmd_none()];
        }
        default: {
            return [state, Cmd_OfPromise_either((id) => getCartById(id), msg.fields[0], (x_3) => x_3, (arg0) => (new Msg(2, arg0)))];
        }
    }
}

export function Cart(cart) {
    const patternInput = useFeliz_React__React_useElmish_Static_645B1FB7(() => init(cart, void 0), (msg, state) => update(msg, state), []);
    const state_1 = patternInput[0];
    const idRef = useReact_useInputRef();
    console.log(some(state_1));
    const children = toList(delay(() => append(singleton_1(createElement("input", {
        ref: idRef,
    })), delay(() => append(singleton_1(createElement("button", {
        onClick: (_arg1) => {
            if (idRef.current != null) {
                patternInput[1](new Msg(0, parse(value_4(idRef.current).value, 511, false, 32)));
            }
        },
    })), delay(() => append(singleton_1(createElement("i", {
        children: [state_1.Cart.CartId],
    })), delay(() => map_1((i) => {
        const value_3 = toText(interpolate("%i%P()%s%P()", [i.ProductId, i.Description]));
        return createElement("p", {
            children: [value_3],
        });
    }, state_1.Products)))))))));
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    });
}

