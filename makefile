SRC = $(wildcard src/**/*.c) $(wildcard src/*.c) $(wildcard src/**/**/*.c) $(wildcard src/**/**/**/*.c)\
	  $(wildcard src/**/*.h) $(wildcard src/*.h) $(wildcard src/**/**/*.h) $(wildcard src/**/**/**/*.h)
FOLDER = $(lastword $(notdir $(CURDIR)))
all:
	gcc -g ${SRC} -o bin\\${FOLDER}.exe -Wall