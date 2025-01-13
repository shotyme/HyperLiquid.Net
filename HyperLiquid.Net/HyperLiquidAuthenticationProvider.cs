using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using Nethereum.Signer.EIP712;
using Nethereum.Util;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Signer;
using Nethereum.Hex.HexConvertors.Extensions;
using Org.BouncyCastle.Utilities.Encoders;
using Nethereum.ABI;
using static System.Collections.Specialized.BitVector32;
using MessagePack;
using Nethereum.ABI.EIP712;
using Secp256k1Net;
using Org.BouncyCastle.Crypto;

namespace HyperLiquid.Net
{
    internal class HyperLiquidAuthenticationProvider : AuthenticationProvider
    {
        public HyperLiquidAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
        }

        public override void AuthenticateRequest(
            RestApiClient apiClient,
            Uri uri,
            HttpMethod method,
            ref IDictionary<string, object>? uriParameters,
            ref IDictionary<string, object>? bodyParameters,
            ref Dictionary<string, string>? headers,
            bool auth,
            ArrayParametersSerialization arraySerialization,
            HttpMethodParameterPosition parameterPosition,
            RequestBodyFormat requestBodyFormat)
        {
            headers = new Dictionary<string, string>() { };

            if (!auth)
                return;

            //var nonce = DateTimeConverter.ConvertToMilliseconds(DateTime.UtcNow).Value;
            //bodyParameters ??= new ParameterCollection();
            //bodyParameters.Add("nonce", nonce);

            //var hash = HashAction(bodyParameters["action"], nonce);
            //var message = new Dictionary<string, object>
            //{
            //    { "source", "a" },
            //    //{ "connectionId", "0x" + BytesToHexString(hash).ToLowerInvariant() }
            //    { "connectionId", BytesToHexString(hash).ToLowerInvariant() }
            //};
            //var data = GetData(message);
            //var serialized = new SystemTextJsonMessageSerializer().Serialize(data);
            //var signature = Sign(serialized);

            //var r = signature.Substring(0, 66);
            //var s = "0x" + signature.Substring(66, 64);
            //var v = int.Parse(signature.Substring(130, 2), System.Globalization.NumberStyles.HexNumber);

            //bodyParameters["signature"] = new Dictionary<string, object> {
            //    { "r", r },
            //    { "s", s },
            //    { "v", v }
            //};

            bodyParameters ??= new ParameterCollection();
            var nonce = DateTimeConverter.ConvertToMilliseconds(DateTime.UtcNow).Value;
            bodyParameters.Add("nonce", nonce);
            var action = bodyParameters["action"];

            object hash = this.actionHash(action, null, nonce);
            object phantomAgent = this.constructPhantomAgent(hash);
            object chainId = 1337; // check this out
            object domain = new Dictionary<string, object>() {
                    { "chainId", chainId },
                    { "name", "Exchange" },
                    { "verifyingContract", "0x0000000000000000000000000000000000000000" },
                    { "version", "1" },
                };
            object messageTypes = new Dictionary<string, object>() {
            { "Agent", new List<object>() {new Dictionary<string, object>() {
            { "name", "source" },
            { "type", "string" },
            }, new Dictionary<string, object>() {
                { "name", "connectionId" },
                { "type", "bytes32" },
            }} },
                };
            object msg = this.ethEncodeStructuredData(domain, messageTypes, phantomAgent);
            object signature = this.signMessage(msg, ApiKey);

            bodyParameters["signature"] = signature;
            //return signature;
        }
        public virtual object signMessage(object message, object privateKey)
        {
            return this.signHash(this.hashMessage(message), slice(privateKey, -64, null));
        }

        public virtual object hashMessage(object message)
        {
            return add("0x", this.hash(message, () => "keccak", "hex"));
        }

        public virtual object signHash(object hash, object privateKey)
        {
            var signature = ecdsa(slice(hash, -64, null), slice(privateKey, -64, null), () => "secp256k1", null);
            return new Dictionary<string, object>() {
            { "r", add("0x", signature["r"]) },
            { "s", add("0x", signature["s"]) },
            //{ "v", 27 + (int)signature["v"] },
            { "v", (int)signature["v"] - 27 },
        };
        }
        public string slice(object str2, object idx1, object idx2) => Slice(str2, idx1, idx2);

        public static string Slice(object str2, object idx1, object idx2)
        {
            if (str2 == null)
            {
                return null;
            }
            var str = (string)str2;
            var start = idx1 != null ? Convert.ToInt32(idx1) : -1;
            if (idx2 == null)
            {
                if (start < 0)
                {
                    var innerStart = str.Length + start;
                    innerStart = innerStart < 0 ? 0 : innerStart;
                    return str.Substring(innerStart);
                }
                else
                {
                    return str.Substring(start);
                }
            }
            else
            {
                var end = Convert.ToInt32(idx2);
                if (start < 0)
                {
                    start = str.Length + start;
                }
                if (end < 0)
                {
                    end = str.Length + end;
                }
                if (end > str.Length)
                {
                    end = str.Length;
                }
                return str.Substring(start, end - start);
            }
        }

        public Dictionary<string, object> ecdsa(object request, object secret, Delegate alg = null, Delegate hash = null) => Ecdsa(request, secret, alg, hash);

        public static Dictionary<string, object> Ecdsa(object request, object secret, Delegate curve = null, Delegate hash = null)
        {
            var curveName = "secp256k1";
            if (curve != null)
            {
                curveName = curve.DynamicInvoke() as String;
            }
            if (curveName != "secp256k1" && curveName != "p256")
            {
                throw new ArgumentException("Only secp256k1 and p256 curves are supported supported");
            }

            var hashName = (hash != null) ? hash.DynamicInvoke() as string : null;
            byte[] msgHash;

            //var seckey = ConvertHexStringToByteArray(secret.ToString());

            int recoveryId;            
                
            msgHash = ConvertHexStringToByteArray((string)request);

            var sig = new Nethereum.Signer.MessageSigner().SignAndCalculateV(msgHash, new EthECKey((string)secret));

            //using var secp256k1 = new Secp256k1();
            //Span<byte> sig = new Span<byte>();
            //secp256k1.Sign(sig, msgHash, seckey); 

            //var rBytes = sig.Slice(0, 32).ToArray();
            //var sBytes = sig.Slice(32, 32).ToArray();

            var rBytes = sig.R;
            var sBytes = sig.S;
            var v = sig.V;

            var r = BytesToHexString(rBytes).ToLowerInvariant();
            var s = BytesToHexString(sBytes).ToLowerInvariant();


            return new Dictionary<string, object>() {
            { "r", r },
            { "s", s },
            { "v", (int)v[0] },
        };
        }

        public object ethEncodeStructuredData(object domain2, object messageTypes2, object messageData2)
        {
            // const domain =({"chainId":1337,"verifyingContract":"0x0000000000000000000000000000000000000000"})
            // const messageTypes = {"Agent":[{"name":"source","type":"uint256"},{"name":"connectionId","type":"bytes32"}]}

            var domain = domain2 as IDictionary<string, object>;
            var messageTypes = messageTypes2 as IDictionary<string, object>;
            var messageTypesKeys = messageTypes.Keys.ToArray();
            var domainValues = domain.Values.ToArray();
            var domainTypes = new Dictionary<string, string>();
            var messageData = messageData2 as IDictionary<string, object>;

            var typeRaw = new TypedDataRaw(); // contains all domain + message info

            // infer types from values
            foreach (var key in domain.Keys)
            {
                // var type = domainValue.GetType();
                var domainValue = domain[key];
                if (domainValue is string && (domainValue as string).StartsWith("0x"))
                    domainTypes.Add(key, "address");
                else if (domainValue is string)
                    domainTypes.Add(key, "string");
                else
                    domainTypes.Add(key, "uint256"); // handle other use cases later
            }

            var types = new Dictionary<string, MemberDescription[]>();

            // fill in domain types
            var domainTypesDescription = new List<MemberDescription> { };
            var domainValuesArray = new List<MemberValue> { };
            var eip721Domain = new List<object[]> { };
            eip721Domain.Add(new object[]{
                "name",
                "string"
        });
            eip721Domain.Add(new object[]{
                "version",
                "string"
        });
            eip721Domain.Add(new object[]{
                "chainId",
                "uint256"
        });
            eip721Domain.Add(new object[]{
                "verifyingContract",
                "address"
        });
            eip721Domain.Add(new object[]{
                "salt",
                "bytes32"
        });
            foreach (var d in eip721Domain)
            {
                var key = d[0] as string;
                var type = d[1] as string;
                for (var i = 0; i < domain.Count; i++)
                {
                    if (String.Equals(key, domain.Keys.ElementAt(i)))
                    {
                        var value = domainValues[i];
                        var memberDescription = new MemberDescription();
                        memberDescription.Name = key;
                        memberDescription.Type = type;
                        domainTypesDescription.Add(memberDescription);

                        var memberValue = new MemberValue();
                        memberValue.TypeName = type;
                        memberValue.Value = value;
                        domainValuesArray.Add(memberValue);
                    }
                }
            }
            types["EIP712Domain"] = domainTypesDescription.ToArray();
            typeRaw.DomainRawValues = domainValuesArray.ToArray();

            // fill in message types
            var messageTypesDict = new Dictionary<string, string>();
            var typeName = messageTypesKeys[0];
            var messageTypesContent = messageTypes[typeName] as IList<object>;
            var messageTypesDescription = new List<MemberDescription> { };
            for (var i = 0; i < messageTypesContent.Count; i++)
            {
                var elem = messageTypesContent[i] as IDictionary<string, object>; // {\"name\":\"source\",\"type\":\"string\"}
                var name = elem["name"] as string;
                var type = elem["type"] as string;
                messageTypesDict[name] = type;
                // var key = messageTypesContent.Keys.ElementAt(i);
                // var value = messageTypesContent.Values.ElementAt(i);
                var member = new MemberDescription();
                member.Name = name;
                member.Type = type;
                messageTypesDescription.Add(member);
            }
            types[typeName] = messageTypesDescription.ToArray();

            // fill in message values
            var messageValues = new List<MemberValue> { };
            for (var i = 0; i < messageData.Count; i++)
            {

                var key = messageData.Keys.ElementAt(i);// for instance source
                var type = messageTypesDict[key];
                var value = messageData.Values.ElementAt(i); // 1
                var member = new MemberValue();
                member.TypeName = type;
                if (type == "bytes32" && value is string)
                {
                    var hexString = value as string;
                    if (hexString.StartsWith("0x"))
                    {
                        hexString = hexString.Substring(2);
                    }

                    // Convert the hex string to a byte array
                    byte[] byteArray = Enumerable.Range(0, hexString.Length)
                                                 .Where(x => x % 2 == 0)
                                                 .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                                                 .ToArray();
                    member.Value = byteArray;
                }
                else if (type == "bytes32[]")
                {
                    var hexStrings = value as IList<object>;
                    var byteArray = new List<byte[]> { };
                    foreach (var hex2 in hexStrings)
                    {
                        var hex = hex2 as string;
                        if (hex.StartsWith("0x"))
                        {
                            hex = hex.Substring(2);
                        }

                        // Convert the hex string to a byte array
                        var byteArrayElem = Enumerable.Range(0, hex.Length)
                                                     .Where(x => x % 2 == 0)
                                                     .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                                                     .ToArray();
                        byteArray.Add(byteArrayElem);
                    }
                    member.Value = byteArray.ToArray();
                }
                else
                {
                    member.Value = value;
                }
                messageValues.Add(member);
            }
            typeRaw.Message = messageValues.ToArray();
            typeRaw.Types = types;
            typeRaw.PrimaryType = typeName;
            var typedEncoder = new Eip712TypedDataSigner();

            var encodedFromRaw = typedEncoder.EncodeTypedDataRaw((typeRaw));

            return encodedFromRaw;
        }

        private object Packb(object data)
        {
            var packer = new MiniMessagePacker();
            var r1=  packer.Pack(data);
            //var r2 = MessagePackSerializer.Serialize(data);
            return r1;
        }

        private string binaryToBase16(object buff2)
        {
            var buff = (byte[])buff2;
            return binaryToHex(buff);
        }

        public static string binaryToHex(byte[] buff)
        {
            var result = string.Empty;
            foreach (var t in buff)
                result += t.ToString("X2");
            // return result;
            return result.ToLower();// check this
        }

        public string intToBase16(object number)
        {
            var n = Convert.ToInt64(number);
            return n.ToString("x");
        }

        private object actionHash(object action, object vaultAddress, object nonce)
        {
            object dataBinary = this.Packb(action);
            object dataHex = this.binaryToBase16(dataBinary);
            object data = dataHex;
            data = add(data, add("00000", this.intToBase16(nonce)));
            if (true)
            {
                data = add(data, "00");
            }
            else
            {
                data = add(data, "01");
                data = add(data, vaultAddress);
            }
            return this.hash(this.base16ToBinary(data), () => "keccak", "binary");
        }

        public static object add(object a, object b)
        {
            if (a is (Int64))
            {
                return (Int64)a + (Int64)b;
            }
            else if (a is (double))
            {
                return (double)a + Convert.ToDouble(b);
            }
            else if (a is (string))
            {
                return (string)a + (string)b;
            }
            else
            {
                return null;
            }
        }

        public object base16ToBinary(object str2)
        {
            //return (string)str; // stub
            //return Convert.FromHexString((string)str);
            var str = (string)str2;
            return ConvertHexStringToByteArray(str);
        }

        public static byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException("The hex string must have an even number of characters.", nameof(hexString));
            }

            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
            {
                string hexSubstring = hexString.Substring(i, 2);
                bytes[i / 2] = Convert.ToByte(hexSubstring, 16);
            }

            return bytes;
        }

        public object hash(object request2, Delegate algorithm2 = null, object digest2 = null) => Hash(request2, algorithm2, digest2);
        public static object Hash(object request2, Delegate hash = null, object digest2 = null)
        {
            var request = request2 as String;
            var algorithm = hash.DynamicInvoke() as string;
            digest2 ??= "hex";
            var digest = digest2 as String;
            var signature = new Byte[] { };
            signature = SignKeccak(request2);
            if (digest == "binary")
            {
                return signature;
            }
            return digest == "hex" ? binaryToHex(signature) : BytesToBase64String(signature);
        }

        public static byte[] SignKeccak(object data2)
        {
            byte[] msg;
            if (data2 is string)
            {
                msg = Encoding.UTF8.GetBytes((string)data2);
            }
            else
            {
                msg = data2 as byte[];
            }
            Sha3Keccack keccack = new Sha3Keccack();
            var hash = keccack.CalculateHash(msg);
            return hash;
        }

        private object constructPhantomAgent(object hash)
        {
            object source = "a";
            return new Dictionary<string, object>() {
            { "source", source },
            { "connectionId", hash },
        };
        }










        private string Sign(string data)
        {
            var signer = new EthereumMessageSigner();
            var signature = signer.EncodeUTF8AndSign(data, new EthECKey(ApiKey));
            return signature;

            var x = new Nethereum.Signer.EIP712.Eip712TypedDataSigner();
            var key = new Nethereum.Signer.EthECKey(ApiKey);
            var fds = key.GetPublicAddress();
            var result = x.SignTypedDataV4(data, key);
            var y = x.RecoverFromSignatureV4(data, result);
            return result;
        }

        private byte[] HashAction(object action, long nonce)
        {
            //var actionBytes = MessagePackSerializer.Serialize(action);
            var serialized = new SystemTextJsonMessageSerializer().Serialize(action);
            var actionBytes = Encoding.UTF8.GetBytes(serialized);
            var nonceBytes = BitConverter.GetBytes(nonce);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(nonceBytes);
            var result = actionBytes.Concat(nonceBytes).Concat(new[] { (byte)0 }).ToArray();
            return Sha3Keccack.Current.CalculateHash(result);

            //var digest = new KeccakDigest(256);

            //digest.BlockUpdate(result, 0, result.Length);
            //var calculatedHash = new byte[digest.GetByteLength()];
            //digest.DoFinal(calculatedHash, 0);

            //return calculatedHash.Take(32).ToArray();
        }

        private object GetData(object message)
        {
            return new Dictionary<string, object>
            {
                {
                    "domain",
                    new Dictionary<string, object>
                    {
                        { "chainId", 1337 },
                        { "name", "Exchange" },
                        { "verifyingContract", "0x0000000000000000000000000000000000000000" },
                        { "version", "1" },
                    }
                },
                {
                    "types",
                    new Dictionary<string, object>
                    {
                        {
                            "Agent",
                            new []
                            {
                                new Dictionary<string, object>
                                {
                                    { "name", "source" },
                                    { "type", "string" },
                                },
                                new Dictionary<string, object>
                                {
                                    { "name", "connectionId" },
                                    { "type", "bytes32" },
                                }
                            }
                        },
                        {
                            "EIP712Domain",
                            new[]
                            {
                                new Dictionary<string, object>
                                {
                                    { "name", "name" },
                                    { "type", "string" },
                                },
                                new Dictionary<string, object>
                                {
                                    { "name", "version" },
                                    { "type", "string" },
                                },
                                new Dictionary<string, object>
                                {
                                    { "name", "chainId" },
                                    { "type", "uint256" },
                                },
                                new Dictionary<string, object>
                                {
                                    { "name", "verifyingContract" },
                                    { "type", "address" },
                                },
                            }
                        }
                    }
                },
                {
                    "primaryType",
                    "Agent"
                },
                {
                    "message",
                    message
                }
            };
        }
    }
}
