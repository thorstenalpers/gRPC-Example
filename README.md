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

gRPC Upload Average: 236.22 ms

gRPC Upload Times (ms):

```
901, 260, 249, 249, 246, 243, 236, 233, 229, 
223, 231, 227, 226, 221, 223, 224, 239, 237, 
236, 220, 221, 226, 223, 219, 221, 221, 222, 
231, 228, 226, 226, 237, 231, 225, 227, 225, 
229, 235, 219, 224, 223, 224, 222, 231, 228, 
226, 225, 224, 227, 224, 225, 226, 227, 226, 
227, 222, 245, 244, 228, 233, 248, 244, 227, 
224, 237, 247, 234, 238, 238, 238, 233, 228, 
236, 238, 232, 226, 227, 226, 229, 232, 225, 
224, 238, 220, 226, 225, 222, 227, 222, 224,
225, 227, 226, 225, 225, 227, 232, 228, 222, 
224
```
### HTTP

HTTP Upload Average: 735.15 ms

HTTP Upload Times (ms):

```
735, 784, 749, 731, 722, 750, 790, 744, 734, 
742, 736, 739, 736, 733, 756, 736, 725, 727, 
722, 730, 734, 721, 719, 742, 721, 723, 728, 
725, 732, 730, 740, 728, 711, 741, 725, 732,
712, 709, 720, 721, 726, 735, 720, 738, 724, 
715, 717, 723, 728, 726, 733, 735, 712, 725,
727, 731, 735, 734, 733, 746, 736, 740, 715,
722, 721, 731, 725, 730, 713, 733, 758, 745, 
717, 739, 727, 745, 733, 732, 776, 759, 734,
748, 756, 752, 760, 750, 742, 747, 744, 750,
740, 742, 755, 751, 744, 744, 746, 737, 738, 
740
```
```
Summary:
  Number of tries: 100
  File size: 50 MB
  HTTP Average Upload Time: 735.15 ms
  gRPC Average Upload Time: 236.22 ms  
  gRPC is 3.11x faster than HTTP
```
