import { createElement } from "react";
import * as react from "react";
import { SignalRHub_Action$reflection, SignalRHub_Response$reflection, SignalRHub_Action } from "./MyStore.Domain/Chat.js";
import { React_createDisposable_3A5B6456, useReact_useEffectOnce_Z5ECA432F, useReact_useMemo_CF4EA67, useReact_useRef_1505, useFeliz_React__React_useState_Static_1505 } from "./.fable/Feliz.1.45.0/React.fs.js";
import { HubConnectionBuilder$5_$ctor_Z66CB2AA1, HubConnectionBuilder$5__withUrl_Z721C83C5, HubConnectionBuilder$5__withAutomaticReconnect, HubConnectionBuilder$5__configureLogging_2D37BB17, HubConnectionBuilder$5__onMessage_2163CAFC } from "./.fable/Fable.SignalR.0.11.4/SignalR.fs.js";
import { HubConnection$5__stopNow, HubConnection$5__startNow, HubConnection$5_$ctor_3ED56BCC, Bindings_signalR } from "./.fable/Fable.SignalR.0.11.4/HubConnection.fs.js";
import { Json_TextMessageFormat_write, Json_TextMessageFormat_parse, HubRecords_CloseMessage$reflection, HubRecords_PingMessage$reflection, HubRecords_CancelInvocationMessage$reflection, HubRecords_StreamInvocationMessage$1$reflection, HubRecords_CompletionMessage$1$reflection, HubRecords_StreamItemMessage$1$reflection, HubRecords_InvocationMessage$1$reflection, Json_JsonProtocol_$ctor, MsgPack_parseMsg, MsgPack_MsgPackProtocol_$ctor } from "./.fable/Fable.SignalR.0.11.4/Protocols.fs.js";
import { toFail, printf, toText } from "./.fable/fable-library.3.1.11/String.js";
import { singleton, reverse, cons, empty } from "./.fable/fable-library.3.1.11/List.js";
import { Reader__Read_24524716, Reader_$ctor_6C95DA22 } from "./.fable/Fable.Remoting.MsgPack.1.8.0/Read.fs.js";
import { fromInteger, op_Subtraction, compare, fromBits, op_Addition } from "./.fable/fable-library.3.1.11/Long.js";
import { enum_type, int32_type, unit_type, class_type } from "./.fable/fable-library.3.1.11/Reflection.js";
import { InvokeArg$1$reflection, MsgPack_Msg$4, MsgPack_Msg$4$reflection } from "./.fable/Fable.SignalR.0.11.4/Shared.fs.js";
import { writeObject } from "./.fable/Fable.Remoting.MsgPack.1.8.0/Write.fs.js";
import { choose, addRangeInPlace } from "./.fable/fable-library.3.1.11/Array.js";
import { equals } from "./.fable/fable-library.3.1.11/Util.js";
import { SimpleJson_readPath, SimpleJson_parse } from "./.fable/Fable.SimpleJson.3.19.0/SimpleJson.fs.js";
import { some, map, value as value_11 } from "./.fable/fable-library.3.1.11/Option.js";
import { Fable_SimpleJson_Json__Json_stringify_Static_4E60E31B, Convert_fromJson } from "./.fable/Fable.SimpleJson.3.19.0/Json.Converter.fs.js";
import { createTypeInfo } from "./.fable/Fable.SimpleJson.3.19.0/TypeInfo.Converter.fs.js";
import { Result_Map, FSharpResult$2 } from "./.fable/fable-library.3.1.11/Choice.js";
import { Interop_reactApi } from "./.fable/Feliz.1.45.0/Interop.fs.js";

export function TextDisplay(input) {
    return react.createElement(react.Fragment, {}, createElement("div", {
        children: [input.count],
    }), createElement("div", {
        children: [input.text],
    }));
}

export function Buttons(input) {
    return react.createElement(react.Fragment, {}, createElement("button", {
        children: "Increment",
        onClick: (_arg1) => {
            input.hub.sendNow(new SignalRHub_Action(0, input.count));
        },
    }), createElement("button", {
        children: "Decrement",
        onClick: (_arg2) => {
            input.hub.sendNow(new SignalRHub_Action(1, input.count));
        },
    }), createElement("button", {
        children: "Get Random Character",
        onClick: (_arg3) => {
            input.hub.sendNow(new SignalRHub_Action(1, 10));
        },
    }));
}

export function TrueChat() {
    const patternInput = useFeliz_React__React_useState_Static_1505(0);
    const count = patternInput[0] | 0;
    const patternInput_1 = useFeliz_React__React_useState_Static_1505("");
    let hub_2;
    const connection_1 = useReact_useRef_1505(useReact_useMemo_CF4EA67(() => {
        let protocol, protocol_1;
        const _ = HubConnectionBuilder$5__onMessage_2163CAFC(HubConnectionBuilder$5__configureLogging_2D37BB17(HubConnectionBuilder$5__withAutomaticReconnect(HubConnectionBuilder$5__withUrl_Z721C83C5(HubConnectionBuilder$5_$ctor_Z66CB2AA1(new Bindings_signalR.HubConnectionBuilder()), "/Support/Chat/Ws")), 1), (_arg1) => {
            if (_arg1.tag === 1) {
                patternInput_1[1](_arg1.fields[0]);
            }
            else {
                patternInput[1](_arg1.fields[0]);
            }
        });
        return HubConnection$5_$ctor_3ED56BCC(_["hub@10"].withHubProtocol(_.useMsgPack ? (protocol = MsgPack_MsgPackProtocol_$ctor(), {
            name: "messagepack",
            version: 1,
            transferFormat: 2,
            parseMessages(input, logger) {
                return Array.from((() => {
                    let arg10_1;
                    try {
                        const buffer_1 = input;
                        const reader = Reader_$ctor_6C95DA22(new Uint8Array(buffer_1));
                        const read = (pos_mut, xs_mut) => {
                            read:
                            while (true) {
                                const pos = pos_mut, xs = xs_mut;
                                const matchValue = op_Addition(op_Addition(Reader__Read_24524716(reader, class_type("System.UInt64")), pos), fromBits(1, 0, true));
                                if (compare(op_Subtraction(fromInteger(buffer_1.byteLength, true, 2), matchValue), fromBits(0, 0, true)) > 0) {
                                    pos_mut = matchValue;
                                    xs_mut = cons(MsgPack_parseMsg(Reader__Read_24524716(reader, MsgPack_Msg$4$reflection(unit_type, SignalRHub_Response$reflection(), SignalRHub_Response$reflection(), unit_type))), xs);
                                    continue read;
                                }
                                else {
                                    return cons(MsgPack_parseMsg(Reader__Read_24524716(reader, MsgPack_Msg$4$reflection(unit_type, SignalRHub_Response$reflection(), SignalRHub_Response$reflection(), unit_type))), xs);
                                }
                                break;
                            }
                        };
                        return reverse(read(fromBits(0, 0, true), empty()));
                    }
                    catch (e) {
                        logger.log(4, (arg10_1 = e.message, toText(printf("An error occured during message deserialization: %s"))(arg10_1)));
                        return empty();
                    }
                })());
            },
            writeMessage(msg_2) {
                let matchValue_1, invocation, matchValue_2, invocation_1, arg10_3, streamItem, completion, streamInvocation, cancelInvocation, close;
                const message = msg_2;
                const outArr = [];
                writeObject((matchValue_1 = (message.type | 0), (matchValue_1 === 1) ? (invocation = message, (matchValue_2 = invocation.target, (matchValue_2 === "Invoke") ? ((invocation.arguments.length === 2) ? (new MsgPack_Msg$4(1, invocation.headers, invocation.invocationId, invocation.target, invocation.arguments[0], invocation.arguments[1], invocation.streamIds)) : (invocation_1 = message, new MsgPack_Msg$4(2, invocation_1.headers, invocation_1.invocationId, invocation_1.target, invocation_1.arguments, invocation_1.streamIds))) : ((matchValue_2 === "Send") ? (new MsgPack_Msg$4(0, invocation.headers, invocation.invocationId, invocation.target, invocation.arguments, invocation.streamIds)) : ((matchValue_2 === "StreamTo") ? (new MsgPack_Msg$4(0, invocation.headers, invocation.invocationId, invocation.target, invocation.arguments, invocation.streamIds)) : (arg10_3 = invocation.target, toFail(printf("Invalid Invocation Target: %s"))(arg10_3)))))) : ((matchValue_1 === 2) ? (streamItem = message, new MsgPack_Msg$4(3, streamItem.headers, streamItem.invocationId, streamItem.item)) : ((matchValue_1 === 3) ? (completion = message, new MsgPack_Msg$4(4, completion.headers, completion.invocationId, completion.error, completion.result)) : ((matchValue_1 === 4) ? (streamInvocation = message, new MsgPack_Msg$4(5, streamInvocation.headers, streamInvocation.invocationId, streamInvocation.target, streamInvocation.arguments, streamInvocation.streamIds)) : ((matchValue_1 === 5) ? (cancelInvocation = message, new MsgPack_Msg$4(6, cancelInvocation.headers, cancelInvocation.invocationId)) : ((matchValue_1 === 6) ? (new MsgPack_Msg$4(7)) : ((matchValue_1 === 7) ? (close = message, new MsgPack_Msg$4(8, close.error, close.allowReconnect)) : toFail(printf("Invalid message: %A"))(message)))))))), MsgPack_Msg$4$reflection(unit_type, SignalRHub_Action$reflection(), unit_type, unit_type), outArr);
                if (compare(fromInteger(outArr.length, true, 2), fromBits(2147483648, 0, true)) > 0) {
                    throw (new Error("Messages over 2GB are not supported."));
                }
                else {
                    const msgArr = [];
                    writeObject(fromInteger(outArr.length, true, 2), class_type("System.UInt64"), msgArr);
                    addRangeInPlace(outArr, msgArr);
                    return (new Uint8Array(msgArr)).buffer;
                }
            },
        }) : (protocol_1 = Json_JsonProtocol_$ctor(), {
            name: "json",
            version: 1,
            transferFormat: 1,
            parseMessages(input_1, logger_2) {
                const input_2 = input_1;
                const logger_3 = logger_2;
                return Array.from(((typeof input_2) === "string") ? (equals(input_2, "") ? [] : (() => {
                    let arg10_9;
                    try {
                        return choose((m) => {
                            let msg_4;
                            const parsedRaw = SimpleJson_parse(m);
                            let _arg2;
                            const parsedRaw_1 = parsedRaw;
                            const msgType_1 = value_11(map((arg00_5) => Convert_fromJson(arg00_5, createTypeInfo(enum_type("Fable.SignalR.Messages.MessageType", int32_type, [["Invocation", 1], ["StreamItem", 2], ["Completion", 3], ["StreamInvocation", 4], ["CancelInvocation", 5], ["Ping", 6], ["Close", 7]]))), SimpleJson_readPath(singleton("type"), parsedRaw))) | 0;
                            switch (msgType_1) {
                                case 1: {
                                    let _arg1_1;
                                    try {
                                        _arg1_1 = (new FSharpResult$2(0, Convert_fromJson(parsedRaw_1, createTypeInfo(HubRecords_InvocationMessage$1$reflection(SignalRHub_Response$reflection())))));
                                    }
                                    catch (ex) {
                                        _arg1_1 = (new FSharpResult$2(1, ex.message));
                                    }
                                    _arg2 = ((_arg1_1.tag === 1) ? Result_Map((arg_1) => {
                                        let msg_6;
                                        return msg_6 = arg_1, ((msg_6.target === "") ? (() => {
                                            throw (new Error("Invalid payload for Invocation message."));
                                        })() : (void 0), ((msg_6.invocationId != null) ? ((value_11(msg_6.invocationId) === "") ? (() => {
                                            throw (new Error("Invalid payload for Invocation message."));
                                        })() : (void 0)) : (void 0), msg_6));
                                    }, (() => {
                                        try {
                                            return new FSharpResult$2(0, Convert_fromJson(parsedRaw_1, createTypeInfo(HubRecords_InvocationMessage$1$reflection(InvokeArg$1$reflection(SignalRHub_Response$reflection())))));
                                        }
                                        catch (ex_1) {
                                            return new FSharpResult$2(1, ex_1.message);
                                        }
                                    })()) : (new FSharpResult$2(0, (msg_4 = _arg1_1.fields[0], ((msg_4.target === "") ? (() => {
                                        throw (new Error("Invalid payload for Invocation message."));
                                    })() : (void 0), ((msg_4.invocationId != null) ? ((value_11(msg_4.invocationId) === "") ? (() => {
                                        throw (new Error("Invalid payload for Invocation message."));
                                    })() : (void 0)) : (void 0), msg_4))))));
                                    break;
                                }
                                case 2: {
                                    _arg2 = Result_Map((arg_3) => {
                                        let msg_8, matchValue_3, invocationId_1;
                                        return msg_8 = arg_3, (matchValue_3 = msg_8.invocationId, (matchValue_3 != null) ? ((matchValue_3 !== "") ? (invocationId_1 = matchValue_3, msg_8) : (() => {
                                            throw (new Error("Invalid payload for StreamItem message."));
                                        })()) : (() => {
                                            throw (new Error("Invalid payload for StreamItem message."));
                                        })());
                                    }, (() => {
                                        try {
                                            return new FSharpResult$2(0, Convert_fromJson(parsedRaw_1, createTypeInfo(HubRecords_StreamItemMessage$1$reflection(unit_type))));
                                        }
                                        catch (ex_2) {
                                            return new FSharpResult$2(1, ex_2.message);
                                        }
                                    })());
                                    break;
                                }
                                case 3: {
                                    _arg2 = Result_Map((arg_5) => {
                                        let msg_10, fail, matchValue_4, err;
                                        return msg_10 = arg_5, (fail = (() => {
                                            throw (new Error("Invalid payload for Completion message."));
                                        }), ((matchValue_4 = [msg_10.result, msg_10.error], (matchValue_4[0] == null) ? ((matchValue_4[1] != null) ? (err = matchValue_4[1], (err === "") ? fail() : (void 0)) : ((msg_10.invocationId === "") ? fail() : (void 0))) : ((matchValue_4[1] != null) ? fail() : ((msg_10.invocationId === "") ? fail() : (void 0)))), msg_10));
                                    }, (() => {
                                        try {
                                            return new FSharpResult$2(0, Convert_fromJson(parsedRaw_1, createTypeInfo(HubRecords_CompletionMessage$1$reflection(SignalRHub_Response$reflection()))));
                                        }
                                        catch (ex_3) {
                                            return new FSharpResult$2(1, ex_3.message);
                                        }
                                    })());
                                    break;
                                }
                                case 4: {
                                    _arg2 = Result_Map((arg_6) => arg_6, (() => {
                                        try {
                                            return new FSharpResult$2(0, Convert_fromJson(parsedRaw_1, createTypeInfo(HubRecords_StreamInvocationMessage$1$reflection(unit_type))));
                                        }
                                        catch (ex_4) {
                                            return new FSharpResult$2(1, ex_4.message);
                                        }
                                    })());
                                    break;
                                }
                                case 5: {
                                    _arg2 = Result_Map((arg_7) => arg_7, (() => {
                                        try {
                                            return new FSharpResult$2(0, Convert_fromJson(parsedRaw_1, createTypeInfo(HubRecords_CancelInvocationMessage$reflection())));
                                        }
                                        catch (ex_5) {
                                            return new FSharpResult$2(1, ex_5.message);
                                        }
                                    })());
                                    break;
                                }
                                case 6: {
                                    _arg2 = Result_Map((arg_8) => arg_8, (() => {
                                        try {
                                            return new FSharpResult$2(0, Convert_fromJson(parsedRaw_1, createTypeInfo(HubRecords_PingMessage$reflection())));
                                        }
                                        catch (ex_6) {
                                            return new FSharpResult$2(1, ex_6.message);
                                        }
                                    })());
                                    break;
                                }
                                case 7: {
                                    _arg2 = Result_Map((arg_9) => arg_9, (() => {
                                        try {
                                            return new FSharpResult$2(0, Convert_fromJson(parsedRaw_1, createTypeInfo(HubRecords_CloseMessage$reflection())));
                                        }
                                        catch (ex_7) {
                                            return new FSharpResult$2(1, ex_7.message);
                                        }
                                    })());
                                    break;
                                }
                                default: {
                                    _arg2 = toFail(printf("Invalid message: %A"))(parsedRaw_1);
                                }
                            }
                            if (_arg2.tag === 1) {
                                logger_3.log(4, toText(printf("Unknown message type: %s"))(_arg2.fields[0]));
                                return void 0;
                            }
                            else {
                                return some(_arg2.fields[0]);
                            }
                        }, Json_TextMessageFormat_parse(input_2));
                    }
                    catch (e_2) {
                        logger_3.log(4, (arg10_9 = e_2.message, toText(printf("An error occured during message deserialization: %s"))(arg10_9)));
                        return [];
                    }
                })()) : (logger_3.log(4, "Invalid input for JSON hub protocol. Expected a string, got an array buffer instead."), []));
            },
            writeMessage(msg_12) {
                return Json_TextMessageFormat_write(Fable_SimpleJson_Json__Json_stringify_Static_4E60E31B(msg_12));
            },
        })).build(), _.handlers);
    }, void 0));
    useReact_useEffectOnce_Z5ECA432F(() => {
        let objectArg_1;
        HubConnection$5__startNow(connection_1.current);
        return React_createDisposable_3A5B6456((objectArg_1 = connection_1.current, () => {
            HubConnection$5__stopNow(objectArg_1);
        }));
    });
    hub_2 = useReact_useRef_1505(connection_1.current);
    return createElement("div", {
        children: Interop_reactApi.Children.toArray([TextDisplay({
            count: count,
            text: patternInput_1[0],
        }), createElement(Buttons, {
            count: count,
            hub: hub_2.current,
        })]),
    });
}

export function Chat(chatInputProps) {
    const obj = chatInputProps.obj;
    if ((() => {
        try {
            console.log(some(window.location.origin));
            return true;
        }
        catch (ex) {
            return false;
        }
    })()) {
        return TrueChat();
    }
    else {
        return createElement("div", {});
    }
}

