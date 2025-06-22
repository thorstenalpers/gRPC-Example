# File Upload Performance Comparison: gRPC vs HTTP

This project benchmarks the upload performance of two methods for transferring large files (e.g. 50 MB):

- **gRPC** streaming upload  
- **HTTP** multipart upload  

The goal is to compare throughput and latency between these protocols in a .NET environment.

---

## Project Overview

- Generates a large test file (default 50 MB)
- Uploads the file multiple times (default 100 tries) using both gRPC and HTTP
- Measures and logs the elapsed time for each upload
- Computes and displays averages and speedup factor

The gRPC service uses streaming RPC to send chunks of data, while HTTP uses standard multipart form data upload.

---

## How to Run

1. Clone the repository and open in your favorite C# IDE.
2. Ensure you have a gRPC server running locally at `https://localhost:7275`.
3. Run the client project.
4. Observe console output for detailed upload times and summary statistics.

You can adjust parameters such as:

- Number of tries (`tries` variable)
- Test file size in MB (`fileSizeInMB` variable)
- Server address (`serverAddress` variable)

---

## Sample Test Output

Text file 'testfile.txt' of approx. 50 MB created.
Running 100 upload tests with a 50 MB file...

### gRPC

gRPC Upload Average: 208,72 ms

gRPC Upload Times (ms):

```
312, 267, 254, 239, 237, 239, 234, 231, 243, 222,
244, 228, 227, 231, 224, 197, 198, 191, 198, 201,
199, 199, 198, 196, 196, 190, 202, 189, 198, 200, 
199, 203, 193, 201, 198, 199, 190, 199, 212, 199, 
196, 200, 205, 204, 201, 201, 205, 198, 201, 199, 
192, 221, 203, 203, 213, 201, 205, 206, 194, 195, 
187, 202, 210, 202, 196, 199, 193, 201, 186, 194, 
191, 201, 198, 188, 192, 191, 197, 200, 196, 192, 
195, 191, 199, 195, 193, 192, 203, 223, 224, 227, 
230, 231, 233, 226, 226, 230, 243, 226, 223, 226
```
### HTTP

HTTP Upload Average: 838,86 ms

HTTP Upload Times (ms):

```
1002, 838, 837, 826, 834, 843, 840, 834, 845,
863, 884, 841, 845, 846, 855, 843, 831, 822,
845, 853, 826, 830, 820, 816, 819, 845, 829,
833, 826, 831, 917, 883, 837, 828, 833, 825,
811, 818, 830, 828, 822, 812, 848, 823, 814,
823, 821, 814, 821, 818, 817, 816, 821, 825, 
827, 820, 823, 829, 830, 848, 847, 839, 819, 
891, 898, 862, 852, 865, 862, 864, 845, 860, 
827, 826, 834, 826, 827, 819, 841, 900, 952, 
879, 848, 831, 826, 841, 836, 821, 817, 816, 
814, 827, 836, 824, 827, 820, 817, 822, 821
```

```
Summary:
  Number of tries: 100
  File size: 50 MB
  HTTP Average Upload Time: 838,86 ms
  gRPC Average Upload Time: 208,72 ms
  gRPC is 4,02x faster than HTTP
```
