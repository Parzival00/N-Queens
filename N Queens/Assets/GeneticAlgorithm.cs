//add avg fitness graph
//and max of all time graph

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneticAlgorithm : MonoBehaviour
{
    int generation;
    bool running = false;
    int candidates = 10;
    int candidateSize = 8;
    float keep = 0.5f;
    float mutateRate = 0.2f;
    sample max;
    List<sample> candidatesList = new List<sample>();
    [SerializeField] GameObject queen;
    [SerializeField] GridManager manager;
    [Header ("UI Elements")]
    //[SerializeField] InputField input;
    [SerializeField] Slider speedSlider;
    [SerializeField] Text GenerationText;
    [SerializeField] Text BestHitCount;
    [SerializeField] Text CurrentHitCount;

    public struct sample
    {
        public string code;
        public int eval;
    };

    public void run()
    {
        
        Time.timeScale = 0.05f;
        generate(ref candidatesList);
        max = candidatesList[0];
        //candidateSize = int.Parse(input.text);
        manager.generateBoth();
        running = true;
        generation = 0;
    }
    private void FixedUpdate()
    {
        Time.timeScale = speedSlider.value;
        if (running)
        {
            GenerationText.text = "Current Generation: " + generation++;

            //Evaluate
            evaluate(ref candidatesList);

            //getting the max value
            for (int i = 0; i < candidatesList.Count; i++)
            {
                if (candidatesList[i].eval < max.eval && candidatesList[i].eval != -1)
                {
                    max = candidatesList[i];
                }
            }

            

            //sort
            sort(ref candidatesList);

            BestHitCount.text = "Hit Count: " + max.eval;
            CurrentHitCount.text = "Hit Count: " + candidatesList[0].eval;

            if(max.eval == 0)
            {
                running = false;
            }

            //show results
            clearBoard();
            showResults(candidatesList);

            //cross over
            crossOver(ref candidatesList);

            //mutate
            mutate(ref candidatesList);

        }

    }

    void generate(ref List<sample> samplesIn)
    {
        for (int num = 0; num < candidates; num++)
        {
            sample sample1;

            //generate a code based on candidateSize
            string temp = "";
            for (int i = 0; i < candidateSize; i++)
            {
                temp += ((Random.Range(0,candidateSize))).ToString();
                
            }
            sample1.code = temp;

            //pass the value to this fitness function
            //sample1.eval = f(sample1.value);
            sample1.eval = 10000;

            samplesIn.Add(sample1);
        }

    }

    void evaluate(ref List<sample> samplesIn)
    {
        List<sample> temp = new List<sample>();
        
        for (int i = 0; i < samplesIn.Count; i++)
        {
            sample tempSample = new sample();
            tempSample.code = samplesIn[i].code;
            int hitCount = 0;
            for (int j = 0; j < candidateSize - 1; j++)
            {
                for (int k = 1; k < candidateSize - j; k++)
                {
                    
                    //if(k-j >= 0 && k+j < candidateSize)
                    
                        //diagonal up
                        if ((samplesIn[i].code[j] - '0') == (samplesIn[i].code[j + k] - '0') + k)
                            hitCount++;
                        //diagonal down
                        if ((samplesIn[i].code[j] - '0') == (samplesIn[i].code[j + k] - '0') - k)
                            hitCount++;
                        //horizontal
                        if ((samplesIn[i].code[j] - '0') == (samplesIn[i].code[j + k] - '0'))
                            hitCount++;
                        //we dont need vertical because each code index is a unique column
                    

                }
            }
            
            tempSample.eval = hitCount;
            temp.Add(tempSample);
            
        }
        candidatesList = temp;
        
        
    }

    void sort(ref List<sample> samplesIn)
    {
        for (int i = 0; i < samplesIn.Count - 1; i++)
        {
            for (int j = i; j < samplesIn.Count; j++)
            {
                if (samplesIn[i].eval > samplesIn[j].eval)
                {
                    //switch
                    sample temp = samplesIn[i];
                    samplesIn[i] = samplesIn[j];
                    samplesIn[j] = temp;

                }
            }
        }

    }

    void crossOver(ref List<sample> samplesIn)
    {
        //make temp vector for new samples
        List<sample> newCandidates = new List<sample>();

        //loop on keep percentage of population
        int keepCandidates = (int)(candidates * keep);
        sample sample1;
        sample sample2;
        for (int i = 0; i < candidates / 2; i++)
        {

            //choose 2 from keep population
            
            sample1 = samplesIn[Random.Range(0,samplesIn.Count-1)];
            
            sample2 = samplesIn[Random.Range(0, samplesIn.Count - 1)];



            sample newSample1;
            sample newSample2;

            //choose cut location
            int cut = Random.Range(1, candidateSize-1) ;

            //make two new candidates
            newSample1.code = "";
            newSample2.code = "";
            for (int j = 0; j < candidateSize; j++)
            {
                if (j < cut)
                {
                    newSample1.code += sample1.code[j];
                    newSample2.code += sample2.code[j];
                }
                else
                {
                    newSample1.code += sample2.code[j];
                    newSample2.code += sample1.code[j];
                }
            }

            //save them
            newSample1.eval = -1;
            newSample2.eval = -1;
            newCandidates.Add(newSample1);
            newCandidates.Add(newSample2);
        }

        //replace the old candidate list with the new one
        for (int i = 0; i < candidates; i++)
        {
            candidatesList = newCandidates;
        }

    }

    void mutate(ref List<sample> samplesIn)
    {
        
        List<sample> tempList = new List<sample>();

        for (int i = 0; i < samplesIn.Count; i++)
        {
            if ((Random.Range(0, 100) / (float)100) < mutateRate)
            {
                int location = Random.Range(0, candidateSize);

                sample temp = samplesIn[i];
                char[] tempCode = temp.code.ToCharArray();
                tempCode[location] = (char)(Random.Range(0, candidateSize - 1) + '0');

                temp.code = "";
                foreach (char c in tempCode)
                {
                    temp.code += c;
                }
                
                tempList.Add(temp);

                //samplesIn[i].code[location] = Random.Range(0, candidateSize) + '0';
            }
            else tempList.Add(samplesIn[i]);
        }
        candidatesList = tempList;
    }

    void showResults(List<sample> samplesIn)
    {

        char[] current = samplesIn[0].code.ToCharArray();
        char[] min = max.code.ToCharArray();

        for(int i = 0; i < candidateSize; i++)
        {
            
            Instantiate(queen, new Vector3(i/2.0f, (current[i] - '0')/2.0f, - 1), Quaternion.identity);

            Instantiate(queen, new Vector3(i / 2.0f + 10, (min[i] - '0') / 2.0f, -1), Quaternion.identity);
        }
    }

    void clearBoard()
    {
        GameObject[] queens = FindObjectsOfType<GameObject>();

        foreach(GameObject q in queens)
        {
            if (q.tag == "Queen")
            {
                Destroy(q);
            }
        }
    }

}
