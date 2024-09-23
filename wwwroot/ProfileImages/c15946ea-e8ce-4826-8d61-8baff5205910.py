import nltk
import random
from nltk.corpus import brown
from nltk.tokenize import word_tokenize,sent_tokenize
from collections import defaultdict

nltk.download('punkt')
nltk.download('brown')

class N_gram:
    def __init__ (self):
        self.follow = []

    def append(self, word):
        self.follow.append(word)


class Program:
    def __init__(self, m, n, maxLen, corpus):
        self.m = m
        self.n = n
        self.maxLen = maxLen
        self.corpus = corpus
        self.brown_corpus = []
        self.n_gram_dic = defaultdict(N_gram)

    def data_preprocessing(self):
        for sentence in corpus:
            new_words = self.sentence_preprocessing(sentence)
            self.brown_corpus.append(new_words)

    def sentence_preprocessing(self, sentence):
        new_words = []
        for word in sentence:
            if word.isalnum():
                new_words.append(word.lower())

        return new_words

    def data_preprocessing_input(self):
        for sentence in corpus:
            new_words = self.sentence_preprocessing_input(sentence)
            self.brown_corpus.append(new_words)

    def sentence_preprocessing_input(self, sentence):
        words = word_tokenize(sentence)
        new_words = []
        for word in words:
            if word.isalnum():
                new_words.append(word.lower())

        return new_words

    def build_n_gram_dictionary(self):
        for sentence in self.brown_corpus:
            for ind in range(len(sentence) - self.n + 1):
                temp = []
                j = 0
                while j < self.n - 1:
                    temp.append(sentence[ind + j])
                    j += 1
                key = tuple(temp)
                follow_word = sentence[ind + j]
                self.n_gram_dic[key].append(follow_word)

    def sentence_generator(self):
        sentence = []
        n_gram = random.choice(list(self.n_gram_dic.keys()))

        for word in n_gram:
            if len(sentence) < self.maxLen:
                sentence.append(word)
            else:
                joined_sentence = ' '.join(sentence)
                return joined_sentence

        while True:
            if len(sentence) >= self.maxLen:
                joined_sentence = ' '.join(sentence)
                return joined_sentence
            else:
                if n_gram in self.n_gram_dic:
                    next_word = self.choose(self.n_gram_dic[n_gram])
                    sentence.append(next_word)
                    n_gram = tuple(sentence[-self.n + 1:])
                else:
                    joined_sentence = ' '.join(sentence)
                    return joined_sentence

    def choose(self, ngram):
        next_word = random.choice(ngram.follow)
        return next_word

    def print_ngram_model(self):
        for key, ngram in self.n_gram_dic.items():
            print("Key:", key)
            for next_word in ngram.follow:
                print(f"Next Word: {next_word}")
            print()


m = int(input("Enter the number of sentences to generate: "))
n = int(input("Enter the value of n (2 for bigram, 3 for trigram): "))
while n != 2 and n != 3:
    n = int(input("Enter the value of n (2 for bigram, 3 for trigram): "))
maxLen = int(input("Enter the max number of words in a sentence: "))

flag = input("Want to Enter Corpus? (y/n) ")
while flag != "y" and flag != "n":
    flag = input("Wrong Input. Want to Enter Corpus? (y/n) ")

if flag == 'n':
    corpus = brown.sents()
    program = Program(m, n, maxLen, corpus)
    program.data_preprocessing()
    program.build_n_gram_dictionary()
    # program.print_ngram_model()
    print()
    for i in range(m):
        sentence = program.sentence_generator()
        print("Sentence " + str(i + 1) + ": " + sentence)

elif flag == 'y':
    print("Enter Paragraph:")
    input_paragraph = input()
    corpus = sent_tokenize(input_paragraph)
    print(corpus)
    program = Program(m, n, maxLen, corpus)
    program.data_preprocessing_input()
    program.build_n_gram_dictionary()
    # program.print_ngram_model()
    print()
    for i in range(m):
        sentence = program.sentence_generator()
        print("Sentence " + str(i + 1) + ": " + sentence)




