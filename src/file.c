/*
 * file.c
 * Copyright 2004-2008  Serge van den Boom (svdb@stack.nl)
 *
 * This file is part of the Opera file system driver for Linux.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License version 2,
 * as published by the Free Software Foundation.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 */

#include <linux/types.h>
#include <linux/fs.h>

#include "operafs.h"

//============================================================================


struct file_operations opera_file_operations = {
	.llseek      = generic_file_llseek,
	.read_iter   = generic_file_read_iter,
	.mmap        = generic_file_mmap,
	.splice_read = filemap_splice_read,
};


// ============================================================================


