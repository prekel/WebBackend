import { int, object } from "./.fable/Thoth.Json.4.0.0/Decode.fs.js";
import { some, ofNullable, toNullable } from "./.fable/fable-library.3.1.11/Option.js";
import { uncurry } from "./.fable/fable-library.3.1.11/Util.js";
import { option as option_1 } from "./.fable/Thoth.Json.4.0.0/Encode.fs.js";
import { newGuid } from "./.fable/fable-library.3.1.11/Guid.js";
import { add } from "./.fable/fable-library.3.1.11/Map.js";
import { empty } from "./.fable/Thoth.Json.4.0.0/Extra.fs.js";
import { CaseStrategy, ExtraCoders } from "./.fable/Thoth.Json.4.0.0/Types.fs.js";
import { awaitPromise } from "./.fable/fable-library.3.1.11/Async.js";
import { PromiseBuilder__Delay_62FBFDE1, PromiseBuilder__Run_212F1D4B } from "./.fable/Fable.Promise.2.0.0/Promise.fs.js";
import { interpolate, toText } from "./.fable/fable-library.3.1.11/String.js";
import { promise } from "./.fable/Fable.Promise.2.0.0/PromiseImpl.fs.js";
import { Fetch_get_5760677E } from "./.fable/Thoth.Fetch.2.0.0/Fetch.fs.js";
import { Types_HttpRequestHeaders } from "./.fable/Fable.Fetch.2.1.0/Fetch.fs.js";
import { singleton } from "./.fable/fable-library.3.1.11/List.js";
import { CartModel$reflection } from "./MyStore.Dto/Shop.js";
import { record_type, array_type, union_type, int32_type, obj_type } from "./.fable/fable-library.3.1.11/Reflection.js";
import { Record, Union } from "./.fable/fable-library.3.1.11/Types.js";
import { Product_ToDomain_Z6B097A11, Cart_ToDomain_268DEFC0, Product$reflection, Cart$reflection } from "./MyStore.Domain/Shop.js";
import { map } from "./.fable/fable-library.3.1.11/Array.js";
import { Cmd_OfAsync_start, Cmd_OfAsyncWith_result, Cmd_none } from "./.fable/Fable.Elmish.3.1.0/cmd.fs.js";
import { singleton as singleton_1 } from "./.fable/fable-library.3.1.11/AsyncBuilder.js";
import { useFeliz_React__React_useElmish_Static_645B1FB7 } from "./.fable/Feliz.UseElmish.1.5.1/UseElmish.fs.js";
import { useFeliz_React__React_useState_Static_1505 } from "./.fable/Feliz.1.45.0/React.fs.js";
import { map as map_1, singleton as singleton_2, append, delay, toList } from "./.fable/fable-library.3.1.11/Seq.js";
import { createElement } from "react";
import { parse } from "./.fable/fable-library.3.1.11/Int32.js";
import { Interop_reactApi } from "./.fable/Feliz.1.45.0/Interop.fs.js";

export const Nullable_IntDecoder = (path) => ((v) => object((get$) => toNullable(get$.Optional.Raw(uncurry(2, int))), path, v));

export function Nullable_IntEncoder(nullable) {
    return option_1((value) => value)(ofNullable(nullable));
}

export const extra = new ExtraCoders((() => {
    let copyOfStruct = newGuid();
    return copyOfStruct;
})(), add("System.Nullable`1[System.Int32]", [(nullable) => Nullable_IntEncoder(nullable), Nullable_IntDecoder], empty.Coders));

export function getCartById(id) {
    return awaitPromise(PromiseBuilder__Run_212F1D4B(promise, PromiseBuilder__Delay_62FBFDE1(promise, () => {
        const url = toText(interpolate("https://localhost:5001/Cart/%i%P()", [id]));
        return Fetch_get_5760677E(url, void 0, void 0, singleton(new Types_HttpRequestHeaders(0, "application/json")), new CaseStrategy(1), extra, uncurry(2, void 0), {
            ResolveType: CartModel$reflection,
        }, {
            ResolveType: () => obj_type,
        });
    })));
}

export class Msg extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["Increment", "Fetch", "Fetched"];
    }
}

export function Msg$reflection() {
    return union_type("MyStore.Client.Cart.Msg", [], Msg, () => [[], [["Item", int32_type]], [["Item", CartModel$reflection()]]]);
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

export function fetch1(id) {
    return singleton_1.Delay(() => singleton_1.Bind(getCartById(id), (_arg1) => singleton_1.Return(new Msg(2, _arg1))));
}

export function update(msg, state) {
    switch (msg.tag) {
        case 1: {
            return [state, Cmd_OfAsyncWith_result((x_3) => {
                Cmd_OfAsync_start(x_3);
            }, fetch1(msg.fields[0]))];
        }
        case 2: {
            const patternInput = modelToDomain(msg.fields[0]);
            return [new State(patternInput[0], patternInput[1]), Cmd_none()];
        }
        default: {
            return [state, Cmd_none()];
        }
    }
}

export function Cart(cart) {
    const patternInput = useFeliz_React__React_useElmish_Static_645B1FB7(() => init(cart, void 0), (msg, state) => update(msg, state), []);
    const state_1 = patternInput[0];
    const patternInput_1 = useFeliz_React__React_useState_Static_1505(0);
    console.log(some(state_1));
    const children = toList(delay(() => append(singleton_2(createElement("input", {
        onChange: (ev) => {
            patternInput_1[1](parse(ev.target.value, 511, false, 32));
        },
    })), delay(() => append(singleton_2(createElement("button", {
        onClick: (t) => {
            patternInput[1](new Msg(1, patternInput_1[0]));
        },
    })), delay(() => append(singleton_2(createElement("i", {
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

