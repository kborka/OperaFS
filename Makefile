TARGETS := all install clean
SUBDIRS := src

.PHONY: $(TARGETS) $(SUBDIRS)

$(TARGETS): $(SUBDIRS)

$(SUBDIRS): 
	$(MAKE) -C $@ $(MAKECMDGOALS)
