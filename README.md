# OperaFS

A Linux module for mounting CD-ROMs and ISO images from the 3D0 game system. Code originally written by Serge van der Boom and found at [http://www.stack.nl~svdb/operafs/](https://archive.is/DVsH). Code updated to work with current kernel by Kevin Borka. Tested on Debian against kernels 5.10.0-13-amd64 and 5.17.0-1-amd64.

## Installation

**You will need to have a copy of the Linux headers installed on your machine. Please read your distro's documentation on how to install them.**

Run `$ make` from the top directory followed by `# insmod src/operafs.ko`.

## Usage

Mounting from a CD-ROM:

`# mount -t opera /path/to/cdrom /mount/point`

Mounting from an ISO:

`# mount -t opera -o loop /path/to/iso /mount/point`