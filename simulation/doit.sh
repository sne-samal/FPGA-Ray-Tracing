#!/bin/sh

# cleanup 

rm -rf obj_dir
rm -f ColorGenerator.vcd

#run verilator to translate Verilog into C++
verilator -Wall --cc --trace ColorGenerator.sv --exe ColorGenerator_tb.cpp

# build C++ project via make automatically generated by Verilator
make -j -C obj_dir/ -f VColorGenerator.mk VColorGenerator

# run executable simulation
obj_dir/VColorGenerator