# A lightweight C# library for reading and writing c3d files #

c3d4Sharp is a lightweight C# library provided with sources without any dependencies on external libraries. This library contains a simple reader and writer for loading and producing C3D files without having all data in memory (it's processing files as a stream of positional data (and analog data as well)). It's useful when reading and writing really large C3D files.

It's implemented according to [Motion Lab's](http://c3d.org/pdf/c3dformat_ug.pdf) specification. This library is NOT a full spec implementation (so far), but it covers a large part of the spec.


Main characteristic:
- `C3dReader` does not read the whole file at once. It reads frame by frame after calling the `ReadFrame()` method. This is useful especially in case of large c3d files.

A simple example how to read a c3d file:
```
C3dReader reader = new C3dReader();

if (reader.Open("Sample.c3d")) {
    for (int i = 0; i < reader.GetParameter<Int16>("POINT:FRAMES"); i++)
    {
        // returns an array of all points, it is necessary to call this method in each cycle
        Vector3[] array = reader.ReadFrame(); 

        // Another way of reading points - we can ask for specific point 
        // - you can check defined labels in reader.Labels
        Vector3 spine = reader["Spine"];
 
        // get analog data for this frame
        float value = reader.AnalogData["analog_label", 0 /* from 0 to reader.AnalogChannels*/];
        // OR 
        value = reader.AnalogData[0, 0];
        // OR
        float [,] analogData = reader.AnalogData.Data;

        // do your processing

    }
    // Don't forget to close the reader 
    // - it updates the frames count information in the c3d file header and parameters section
    reader.Close();

}
```

More tutorials or examples upon request (you can submit it as an issue :)).