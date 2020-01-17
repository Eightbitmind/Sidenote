# Drive Provider and Drive Rules

- It seems PowerShell automatically upper-cases the drive name part of a path ("on:\..." -> "ON:\...")
  before it passes the path as an argument to a drive provider method.
- The drive name followed by nothing else but a colon (e.g. "on:") is being automatically expanded to
  the current folder within the respective drive.
 
# Considerations

## Identifiable Nodes as Drive Element

# Ideas

## XPath

A way around the unintuitive paths build out of the GUID-like identifiers?

## ID Compression

Shorten the paths by replacing GUID hex digits with '[0-9a-zA-Z]' (62 character) strings?
Is this perhaps only two characters short of a "base 64" encoding?

## Condition Navigator

Expose drive child elements via a "condition navigator" that only stops on identifiable elements?
