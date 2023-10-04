#include<stdio.h>
#include <iostream>
#include <cstdlib>
#include <cmath>
#include <ctime>
#include <vector>
#include <string>

using namespace std;

int candidates = 10;
int candidateSize = 8;
float keep = 0.5;
float mutateRate = 0.2;
int limit = 10000;

struct sample {
    string code;
    int eval;
};

void generate(vector<sample>& samplesIn);
void showCandidates(vector<sample> samplesIn);
void evaluate(vector<sample>& samplesIn);
void sort(vector<sample>& samplesIn);
void crossOver(vector <sample>& samplesIn);
void mutate(vector<sample>& samplesIn);
void showBoard(sample sampleIn);

int main() {
    cout << "===Genetic algorithm===" << endl;

    //------Initializations------

    //seeds the rand functin with the current time
    srand(time(NULL));

    //Generate
    cout << "Generating..." << endl;
    vector<sample> candidates;
    generate(candidates);
    //showCandidates(candidates);


    sample max = candidates[0];
    for (int loop = 0; loop < limit; loop++) {
        //cout << "Loop: " << loop << "---------\n";

        //Evaluate
        //cout << "Evaluating..." << endl;
        evaluate(candidates);
        //showCandidates(candidates);
        float sum = 0;
        for (int i = 0; i < candidates.size(); i++)
        {
            sum += candidates[i].eval;
            if (candidates[i].eval < max.eval && candidates[i].eval != -1) {
                max = candidates[i];
            }
        }
        float average = sum / candidates.size();
        if(loop % 10 == 0)
            cout << "Generation " << loop << " average fitness: " << average << " Current min: " << max.eval << endl;
        //Sort
        //cout << "Sorting..." << endl;
        sort(candidates);
        //showCandidates(candidates);


        //Cross Over
        //cout << "Crossing Over..." << endl;
        crossOver(candidates);
        //showCandidates(candidates);

        //Mutate
        //cout << "Mutating..." << endl;
        mutate(candidates);
        //showCandidates(candidates);


    }
    cout << "Highest fitness in Run: " << max.code << " = " << max.eval << endl;
    showBoard(max);
    return 0;
}

void generate(vector<sample>& samplesIn) {
    for (int num = 0; num < candidates; num++) {
        sample sample1;

        //generate a code based on candidateSize
        string temp = "";
        for (int i = 0; i < candidateSize; i++) {
            temp += to_string((rand() % candidateSize));
            
        }
        sample1.code = temp;

        //pass the value to this fitness function
        //sample1.eval = f(sample1.value);
        sample1.eval = 10000;

        samplesIn.push_back(sample1);
    }
}

void showCandidates(vector<sample> samplesIn) {
    for (int i = 0; i < samplesIn.size(); i++) {
        cout << samplesIn[i].code << "\t" << samplesIn[i].eval << endl;
    }
    cout << "-----------------------\n";
}


void evaluate(vector<sample>& samplesIn) {
    
    for (int i = 0; i < samplesIn.size(); i++) {
        int hitCount = 0;
        for (int j = 0; j < candidateSize-1; j++) {
            for (int k = 1; k < candidateSize - j; k++) {
                
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
        samplesIn[i].eval = hitCount;
    }
}

void sort(vector<sample>& samplesIn) {
    for (int i = 0; i < samplesIn.size() - 1; i++) {
        for (int j = i; j < samplesIn.size(); j++) {
            if (samplesIn[i].eval > samplesIn[j].eval) {
                //switch
                sample temp = samplesIn[i];
                samplesIn[i] = samplesIn[j];
                samplesIn[j] = temp;

            }
        }
    }
}
void crossOver(vector <sample>& samplesIn) {
    //make temp vector for new samples
    vector<sample> newCandidates;

    //loop on keep percentage of population
    int keepCandidates = (candidates * keep);
    sample sample1;
    sample sample2;
    for (int i = 0; i < candidates / 2; i++) {

        //choose 2 from keep population
        sample1 = samplesIn[(rand() % candidates)];
        int count = 0;
        //do {
        sample2 = samplesIn[(rand() % candidates)];

        

        sample newSample1;
        sample newSample2;

        //choose cut location
        int cut = rand() % (candidateSize - 2) + 1;

        //make two new candidates
        newSample1.code = "";
        newSample2.code = "";
        for (int j = 0; j < candidateSize; j++) {
            if (j < cut) {
                newSample1.code += sample1.code[j];
                newSample2.code += sample2.code[j];
            }
            else {
                newSample1.code += sample2.code[j];
                newSample2.code += sample1.code[j];
            }
        }

        //save them
        newSample1.eval = -1;
        newSample2.eval = -1;
        newCandidates.push_back(newSample1);
        newCandidates.push_back(newSample2);
    }

    //replace the old candidate list with the new one
    for (int i = 0; i < candidates; i++) {
        samplesIn[i] = newCandidates[i];
    }

}

void mutate(vector<sample>& samplesIn) {
    for (int i = 0; i < samplesIn.size(); i++) {
        if (((rand() % 100) / (float)100) < mutateRate) {
            int location = rand() % candidateSize;
            
            samplesIn[i].code[location] = (rand() % candidateSize) + '0';
        }
    }
}

void showBoard(sample sampleIn) {
    for (int i = 0; i < candidateSize; i++) {
        for (int j = 0; j < candidateSize; j++) {
            if (sampleIn.code[i] - '0' == j) {
                cout << 1 << " ";
            }
            else cout << 0<<" ";
        }
        cout << endl;
    }
}