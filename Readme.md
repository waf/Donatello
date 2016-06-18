# Donatello

Donatello is a statically-typed, lisp-like language for the .NET platform. You get the power of the .NET platform, with a nice clean lisp syntax.

At this point, Donatello is just an experimental syntax transformation to [Roslyn syntax trees](https://github.com/dotnet/roslyn), rather than a full-blown, production-ready lisp implementation. This will probably upset people, and it should be considered pre-alpha.

# Examples


## Basic Syntax

Simple function invocation. Note that the return type is `int`

```clojure
(+ 1 2 3 4)
10 :int
```

Function definition. The input and output types are specified:

```clojure
(defn add [a:int b:int] :int
  (+ a b))

(add 1 2)
3 :int
```

Variable definition, with the type specified.

```clojure
(def greeting:string "Hello World")
```

Top-level variable and function definitions are not type inferred. However, local `let` definitions are:

```clojure
(let [a 5
      b 6]
    (+ a b))
11 :int
```

Conditionals:

```clojure
(if true 
    "Hello"
    "Goodbye")
```

## Data structures

Donatello has syntax for several types of [immutable, persistent collections](https://msdn.microsoft.com/en-us/library/mt452182.aspx):

### Immutable Arrays

```clojure
[1 2 3 4]
```

### Immutable Dictionaries

This is an associative data structure mapping `string` to `int`:

```clojure
{"a" 2 "b" 4}
```

### Immutable Sets

```clojure
|1 2 3|
```

## Lambda Expressions

Donatello also supports lambda expressions. Here's the full syntax. Note that the types are inferred:

```clojure
(.Select [1 2 3] (fn [elem] (+ elem 1)))
[2 3 4] :WhereSelectArrayIterator<int, int>
```

The above also demonstrates calling .NET methods. See the .NET Interop section below for more detail. In addition to the full syntax there is a shorthand syntax. A shorthand lambda is enclosed in `\()` and uses positional parameters (`^0`, `^1`, etc):

```clojure
(.Select [1 2 3] \(+ ^0 1))
[2 3 4] :WhereSelectArrayIterator<int, int>
```

## .NET Interop:

Donatello is designed to play nicely with the .NET standard library:

```clojure
(.ToUpper "abc")
"ABC" :string

(.Substring "abc" 1)
"bc" :string

(String.Join ", " ["a" "b" "c"])
"a, b, c" :string

(.AddDays (new DateTime 2016 8 29) 2)
"2016-08-31T00:00:00": DateTime

```

Property access on objects uses `-` instead of `.` to differentiate from method access:

```clojure
(-Length "Hello World")
11 :int
```

Donatello uses .NET's module/namespace system. Like Python, the directory structure corresponds to namespaces, and the file name is the class name. By default, all classes and functions are static. However, if you need to create instances or subclasses, you can use the `instance` form:

```clojure
// in the file src/DoomsdayPlan/Explosive
(instance)
(defn Explode [] :string "Kaboom")

// somewhere else
(.Explode (new DoomsdayPlan.Explosive))
```

The `instance` form can also take a list of base classes and interfaces.

```clojure
(instance IComparable<Explosive>)
```


## Developer notes

- The grammar specification is in Antlr/Donatello.g4
- Run `generate.cmd` to turn the grammar into parser code using ANTLR
- Use the generated visitor pattern to turn abstract syntax tree nodes into Roslyn syntax trees
