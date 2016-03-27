# Registers

Registers adds vim like registers to the windows clipboard. This allows you to
store multiple things on the clipboard at the same time and switch between them.
This can be useful when working in multiple applications where you need to copy
many things from one and paste them in another.

Registers also has a queue mode allowing you to copy multiple things into a
register, then paste them in the same order in which you copied them one after
another.

## Usage

To switch to a register, press both shift keys and then press a letter key.
Pressing `lshift + rshift + j` will move you to the `j` register. Now when you
copy something to the clipboard, it will go in the `j` register. Next you could
move to the `k` register with `lshift + rshift + k` and copy something into the
`k` register. Now move back to the `j` register and paste. It will paste what
was copied into the `j` register.

There is also a "split lines" function. This fills up the current regiser with
the a queue of the lines in that register. For instance if you copied the text:
```
Line 1
Line 2
Line 3
```
You could then press `lshift + rshift + /` to split the lines into a queue. The
next three times you paste, it would paste `Line 1`, `Line 2`, and `Line 3` one
after the other.

## Limitations

Windows does not have any events for the clipboard. Therefore, Registers is
currently limited to only being able to listen for the `ctrl + c`, `ctrl + v`, and
`ctrl + x` key combinations. Selecting copy, cut or paste from a menu will not
work. Monitoring the clipboard for changes would be possible, but would be too
processor/memory intensive for what should be a very simple utility.

