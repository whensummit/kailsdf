﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SevenStarAutoSell.Business.Web.Dfv168.Service {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Scripts {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Scripts() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SevenStarAutoSell.Business.Web.Dfv168.Service.Scripts", typeof(Scripts).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 // Copyright (c) 2005  Tom Wu
        ///// All Rights Reserved.
        ///// See &quot;LICENSE&quot; for details.
        ///
        ///// Basic JavaScript BN library - subset useful for RSA encryption.
        ///
        ///// Bits per digit
        ///var dbits;
        ///
        ///var navigator = {};
        ///navigator.appName = &quot;Microsoft Internet Explorer&quot;;
        ///
        ///// JavaScript engine analysis
        ///var canary = 0xdeadbeefcafe;
        ///var j_lm = ((canary&amp;0xffffff)==0xefcafe);
        ///
        ///// (public) Constructor
        ///function BigInteger(a,b,c) {
        ///  if(a != null)
        ///    if(&quot;number&quot; == typeof a) this.fromNumber(a,b,c);
        ///    else if(b == null &amp;&amp; &quot;string&quot; [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string jsbn {
            get {
                return ResourceManager.GetString("jsbn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 // prng4.js - uses Arcfour as a PRNG
        ///
        ///function Arcfour() {
        ///  this.i = 0;
        ///  this.j = 0;
        ///  this.S = new Array();
        ///}
        ///
        ///// Initialize arcfour context from key, an array of ints, each from [0..255]
        ///function ARC4init(key) {
        ///  var i, j, t;
        ///  for(i = 0; i &lt; 256; ++i)
        ///    this.S[i] = i;
        ///  j = 0;
        ///  for(i = 0; i &lt; 256; ++i) {
        ///    j = (j + this.S[i] + key[i % key.length]) &amp; 255;
        ///    t = this.S[i];
        ///    this.S[i] = this.S[j];
        ///    this.S[j] = t;
        ///  }
        ///  this.i = 0;
        ///  this.j = 0;
        ///}
        ///
        ///function ARC4next() {
        ///  var t;
        ///  this.i = (t [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string prng4 {
            get {
                return ResourceManager.GetString("prng4", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 // Random number generator - requires a PRNG backend, e.g. prng4.js
        ///
        ///// For best results, put code like
        ///// &lt;body onClick=&apos;rng_seed_time();&apos; onKeyPress=&apos;rng_seed_time();&apos;&gt;
        ///// in your main HTML document.
        ///
        ///var rng_state;
        ///var rng_pool;
        ///var rng_pptr;
        ///
        ///// Mix in a 32-bit integer into the pool
        ///function rng_seed_int(x) {
        ///  rng_pool[rng_pptr++] ^= x &amp; 255;
        ///  rng_pool[rng_pptr++] ^= (x &gt;&gt; 8) &amp; 255;
        ///  rng_pool[rng_pptr++] ^= (x &gt;&gt; 16) &amp; 255;
        ///  rng_pool[rng_pptr++] ^= (x &gt;&gt; 24) &amp; 255;
        ///  if(rng_pptr &gt;= rng_psize) rng_pp [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string rng {
            get {
                return ResourceManager.GetString("rng", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 // Depends on jsbn.js and rng.js
        ///
        ///// Version 1.1: support utf-8 encoding in pkcs1pad2
        ///
        ///// convert a (hex) string to a bignum object
        ///function parseBigInt(str,r) {
        ///  return new BigInteger(str,r);
        ///}
        ///
        ///function linebrk(s,n) {
        ///  var ret = &quot;&quot;;
        ///  var i = 0;
        ///  while(i + n &lt; s.length) {
        ///    ret += s.substring(i,i+n) + &quot;\n&quot;;
        ///    i += n;
        ///  }
        ///  return ret + s.substring(i,s.length);
        ///}
        ///
        ///function byte2Hex(b) {
        ///  if(b &lt; 0x10)
        ///    return &quot;0&quot; + b.toString(16);
        ///  else
        ///    return b.toString(16);
        ///}
        ///
        ///// PKCS#1 (type 2, random) pa [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string rsa {
            get {
                return ResourceManager.GetString("rsa", resourceCulture);
            }
        }
    }
}
