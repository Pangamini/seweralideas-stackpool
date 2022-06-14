# Changelog

## [0.2.1]
### Changed
- Made the static instance public.
- BasicStackPool.Finalize is no longer sealed.
## [0.2.0]
### Removed
- StackPoolLibrary was removed completely in favor of using StackPool derived types explicitly. This avoids the potentially ambiguous selection of the pool, together with issues where the StackPool type might not exist on AOT platforms. 
### Changed
- The way how StackPools are used.
  - Use the new syntax
    - `using( ListPool<string>.Get(out var stringList))`
    - `using( ListPool<string>.Get(out List<string> stringList))`
  - Don't use the old syntax
    - `using(StackAlloc.Get<out List<string> stringList)` 

## [0.1.2]
### Added
- StackPoolLibrary now prints a message when making a generic type (which is impossible in AOT environment)
