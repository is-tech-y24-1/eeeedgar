import scala.util.chaining.scalaUtilChainingOps

class ScalaClass {
  def helloWorld(): Unit = {
    println(p(1, 2));
  }

  def average(a: Int, b: Int): Double = (a + b).toDouble / 2
  def double(a : Double) : Double = a * 2

  def p(a : Int, b: Int) : Double = average(a, b).pipe(double)
}
